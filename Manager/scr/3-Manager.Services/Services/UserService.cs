using AutoMapper;
using Manager.Services.Interfaces;
using Manager.Infra.Interfaces;
using Manager.Domain.Entities;
using Manager.Core.Exceptions;
using Manager.Services.DTO;
using System.Threading.Tasks;
using System.Collections.Generic;
//using EscNet.Cryptography.Interfaces;
using EscNet.Hashers.Interfaces.Algorithms;
using Isopoh.Cryptography.Argon2;
using EscNet.IoC.Hashers;
using System;
using System.Linq.Expressions;

namespace Manager.Services.Services{
    public class UserService : IUserService{

        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        //private readonly IRijndaelCryptography _rijndaelCryptography; //mudando para Hash
        private readonly IArgon2IdHasher _hasher;

        public UserService(
            IMapper mapper, 
            IUserRepository userRepository, 
            //IRijndaelCryptography rijndaelCryptography,
            IArgon2IdHasher hasher){
            _mapper = mapper;
            _userRepository = userRepository;
            //_rijndaelCryptography = rijndaelCryptography;
            _hasher = hasher;
        }

        public async Task<UserDTO> Create(UserDTO userDTO){
            var userExists = await _userRepository.GetByEmail(userDTO.Email);

            if (userExists != null)
                throw new DomainException("Já existe email");

            var user = _mapper.Map<User>(userDTO);
            user.Validate();
            //user.SetPassword(_rijndaelCryptography.Encrypt(user.Password)); //criptografando a senha
            user.SetPassword(_hasher.Hash(user.Password)); //fazendo Hash na senha

            var userCreated = await _userRepository.Create(user);

            return _mapper.Map<UserDTO>(userCreated);
        }

        public async Task<UserDTO> Update(UserDTO userDTO){
            var userExists = await _userRepository.Get(userDTO.Id);

            if (userExists == null)
                throw new DomainException("Usuario não encontrado");

            var user = _mapper.Map<User>(userDTO);
            user.Validate();
            //user.SetPassword(_rijndaelCryptography.Encrypt(user.Password)); //criptografando a senha
            user.SetPassword(_hasher.Hash(user.Password));

            var userUpdated = await _userRepository.Update(user);

            return _mapper.Map<UserDTO>(userUpdated);
        }

        public async Task Remove(long id){
            await _userRepository.Remove(id);
        }

        public async Task<UserDTO> Get(long id){
             var user = await _userRepository.Get(id);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> Get(){
            var allUsers = await _userRepository.Get();

            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<List<UserDTO>> SearchByName(string name){
            var allUsers = await _userRepository.SearchByName(name);

            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<List<UserDTO>> SearchByEmail(string email){
            var allUsers = await _userRepository.SearchByEmail(email);

            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<UserDTO> GetByEmail(string email){
            var user = await _userRepository.GetByEmail(email);

            return _mapper.Map<UserDTO>(user);
        }

    }
}