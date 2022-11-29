using Manager.Services.Interfaces;
using AutoMapper;
using Moq;
using Manager.Infra.Interfaces;
using EscNet.Cryptography.Interfaces;
using Manager.Tests.Configuration;
using System.Threading.Tasks;
using Xunit;
using Manager.Services.Services;
using Manager.Services.DTO;
using Manager.Domain.Entities;
using Bogus.DataSets;
using FluentAssertions;

namespace Manager.Tests.Projects.Services{

    public class UserServiceTests{
        // sut = Subject Under Test
        private readonly IUserService _sut;

        //mocks - fingindo algo, imitar algo
        private readonly IMapper _mapper;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRijndaelCryptography> _rijndaelCryptographyMock;


        public UserServiceTests(){
            _mapper = AutoMapperConfiguration.GetConfiguration();
            _userRepositoryMock = new Mock<IUserRepository>();
            _rijndaelCryptographyMock = new Mock<IRijndaelCryptography>();

            _sut = new UserService(mapper: _mapper,
                userRepository: _userRepositoryMock.Object,
                rijndaelCryptography: _rijndaelCryptographyMock.Object
            );
        }

        //Fact - Annotation para indicar um teste
        [Fact(DisplayName = "Create Valid User")]
        //assinatura do teste
        [Trait("Category","Services")]
        //nome dos metodos de teste devem seguir padrões, sendo: NOMEMETODO_CONDICAO_RESULTADOESPERADO
        public async Task Create_WhenUserIsValid_ReturnsUserDTO(){
            //teste são 3 etapas
            //1-Arrange - prepara o teste, organiza
            var userToCreate = new UserDTO{Name = "Cleomilson", Email = "cleomilson@hotmai.com.br", Password = "12345678910"};
            var userCreated = _mapper.Map<User>(userToCreate);
            //lorem é do package bogus, que faz dados falsos, para vc criar dados para utilizar
            var encryptedPassword = new Lorem().Sentence();
            userCreated.ChangePassword(encryptedPassword);
            
            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _rijndaelCryptographyMock.Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>()))
                .ReturnsAsync(() => userCreated);

            //2-Act - realiza a função do teste
            var result = await _sut.Create(userToCreate);
            
            //3-Assert - verificar o resultado esperado
            /* forma sem o FluentAssertions
            Assert.Equal(result, userToCreate);
            */
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userCreated));

        }
    }
}