using Bogus;
using System;
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
using Manager.Tests.Fixtures;
using Manager.Core.Exceptions;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        #region Create

        //Fact - Annotation para indicar um teste
        [Fact(DisplayName = "Create Valid User")]
        //assinatura do teste
        [Trait("Category","Services")]
        //nome dos metodos de teste devem seguir padrões, sendo: NOMEMETODO_CONDICAO_RESULTADOESPERADO
        public async Task Create_WhenUserIsValid_ReturnsUserDTO(){
            //teste são 3 etapas
            //1-Arrange - prepara o teste, organiza
            var userToCreate = UserFixture.CreateValidUserDTO(); //antes da fixture - new UserDTO{Name = "Cleomilson", Email = "cleomilson@hotmai.com.br", Password = "12345678910"};
            var userCreated = _mapper.Map<User>(userToCreate);
            //lorem é do package bogus, que faz dados falsos, para vc criar dados para utilizar
            var encryptedPassword = new Lorem().Sentence();
            userCreated.SetPassword(encryptedPassword);
            
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

        [Fact(DisplayName = "Create When User Exists")]
        [Trait("Category", "Services")]
        public async Task Create_WhenUserExists_ReturnsEmptyOptional(){
            var userToCreate = UserFixture.CreateValidUserDTO();
            var userExists = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => userExists);

            var result = await _sut.Create(userToCreate);


            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userToCreate));
        }

        [Fact(DisplayName = "Create When User is Invalid")]
        [Trait("Category","Services")]
        public void Create_WhenUserIsInvald_ThrowsNewDomainException(){
            var userToCreate = UserFixture.CreateInvalidUserDTO();

            _userRepositoryMock.Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            //delegando a criação da função para fazer um invoke
            Func<Task<UserDTO>> act = async() => {
                return await _sut.Create(userToCreate);
            };

            act.Should()
                .Throw<DomainException>();
        }

        #endregion

        #region Update

        [Fact(DisplayName = "Update Valid User")]
        [Trait("Category","Services")]
        public async Task Update_WhenUserIsValid_ReturnsUserDTO(){
            var oldUser = UserFixture.CreateValidUser();
            var userToUpdate = UserFixture.CreateValidUserDTO(); 
            var userUpdated = _mapper.Map<User>(userToUpdate);
            
            var encryptedPassword = new Lorem().Sentence();
            
            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(() => oldUser);

            _rijndaelCryptographyMock.Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                .ReturnsAsync(() => userUpdated);

            var result = await _sut.Update(userToUpdate);
            
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userUpdated));

        }

        [Fact(DisplayName = "Update When User Exists")]
        [Trait("Category","Services")]
        public void Update_WhenUserExists_ThrowsNewDomainException(){
            var userToUpdate = UserFixture.CreateValidUserDTO();

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            //delegando a criação da função para fazer um invoke
            Func<Task<UserDTO>> act = async() => {
                return await _sut.Update(userToUpdate);
            };

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Não existe nenhum usuário com o id informado!");

        }

        [Fact(DisplayName = "Update When User is Invalid")]
        [Trait("Category","Services")]
        public void Update_WhenUserIsInvald_ThrowsNewDomainException(){
            var oldUser = UserFixture.CreateValidUser();
            var userToUpdate = UserFixture.CreateInvalidUserDTO();

            _userRepositoryMock.Setup(x => x.Get(It.IsAny<long>()))
                .ReturnsAsync(() => oldUser);

            //delegando a criação da função para fazer um invoke
            Func<Task<UserDTO>> act = async() => {
                return await _sut.Update(userToUpdate);
            };

            act.Should()
                .Throw<DomainException>();
        }

        #endregion

        #region Remove

        [Fact(DisplayName = "Remove User")]
        [Trait("Category", "Services")]
        public async Task Remove_WhenUserExists_RemoveUser(){
            var userId = new Randomizer().Int(0, 1000);

            _userRepositoryMock.Setup(x => x.Remove(It.IsAny<int>()))
                .Verifiable();

            await _sut.Remove(userId);

            _userRepositoryMock.Verify(x => x.Remove(userId), Times.Once);
        }

        #endregion

        #region Get

        [Fact(DisplayName = "Get By Id")]
        [Trait("Category","Services")]
        public async Task GetById_WhenUserExists_ReturnsUserDTO(){
            var userId = new Randomizer().Int(0, 1000) ;
            var userFound = UserFixture.CreateValidUser();
            
            _userRepositoryMock.Setup(x => x.Get(userId))
                .ReturnsAsync(() => userFound);

            var result = await _sut.Get(userId);
            
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userFound));

        }

        [Fact(DisplayName = "Get By Id When User Not Exists")]
        [Trait("Category","Services")]
        public async Task GetById_WhenUserNotExists_ReturnsNull(){
            var userId = new Randomizer().Int(0, 1000) ;
            
            _userRepositoryMock.Setup(x => x.Get(userId))
                .ReturnsAsync(() => null);

            var result = await _sut.Get(userId);
            
            result.Should()
                .Be(null);

        }

        [Fact(DisplayName = "Get By Email")]
        [Trait("Category","Services")]
        public async Task GetByEmail_WhenUserExists_ReturnsUserDTO(){
            var userEmail = new Internet().Email();
            var userFound = UserFixture.CreateValidUser();
            
            _userRepositoryMock.Setup(x => x.GetByEmail(userEmail))
                .ReturnsAsync(() => userFound);

            var result = await _sut.GetByEmail(userEmail);
            
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userFound));
        }

        [Fact(DisplayName = "Get By Email When User Not Exists")]
        [Trait("Category","Services")]
        public async Task GetByEmail_WhenUserNotExists_ReturnsNull(){
            var userEmail = new Internet().Email();
            
            _userRepositoryMock.Setup(x => x.GetByEmail(userEmail))
                .ReturnsAsync(() => null);

            var result = await _sut.GetByEmail(userEmail);
            
            result.Should()
                .Be(null);

        }

        [Fact(DisplayName = "Get All Users")]
        [Trait("Category","Services")]
        public async Task GetAllUsers_WhenUserExists_ReturnsAListOfUserDTO(){
            var usersFound = UserFixture.CreateListValidUser();
            
            _userRepositoryMock.Setup(x => x.Get())
                .ReturnsAsync(() => usersFound);

            var result = await _sut.Get();
            
            result.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Get All Users When None User Found")]
        [Trait("Category","Services")]
        public async Task GetAllUsers_WhenNoneUserFound_ReturnsEmptyList(){
            
            _userRepositoryMock.Setup(x => x.Get())
                .ReturnsAsync(() => null);

            var result = await _sut.Get();
            
            result.Should()
                .BeEmpty(null);
        }

        #endregion

        #region Search

        [Fact(DisplayName = "Search By Name")]
        [Trait("Category", "Services")]
        public async Task SearchByName_WhenAnyUserFound_ReturnsAListOfUserDTO(){
            var nameToSearch = new Name().FirstName();
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.SearchByName(nameToSearch))
                .ReturnsAsync(() => usersFound);

            var result = await _sut.SearchByName(nameToSearch);

            result.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Search By Name When None User Found")]
        [Trait("Category", "Services")]
        public async Task SearchByName_WhenNoneUserFound_ReturnsEmptyList(){
            var nameToSearch = new Name().FirstName();

            _userRepositoryMock.Setup(x => x.SearchByName(nameToSearch))
                .ReturnsAsync(() => null);

            var result = await _sut.SearchByName(nameToSearch);

            result.Should()
                .BeEmpty();
        }

        [Fact(DisplayName = "Search By Email")]
        [Trait("Category", "Services")]
        public async Task SearchByEmail_WhenAnyUserFound_ReturnsAListOfUserDTO(){
            var emailSoSearch = new Internet().Email();
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.SearchByEmail(emailSoSearch))
                .ReturnsAsync(() => usersFound);

            var result = await _sut.SearchByEmail(emailSoSearch);

            result.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Search By Email When None User Found")]
        [Trait("Category", "Services")]
        public async Task SearchByEmail_WhenNoneUserFound_ReturnsEmptyList(){
            var emailSoSearch = new Internet().Email();

            _userRepositoryMock.Setup(x => x.SearchByEmail(emailSoSearch))
                .ReturnsAsync(() => null);

            var result = await _sut.SearchByEmail(emailSoSearch);

            result.Should()
                .BeEmpty();
        }

        #endregion
    }
}