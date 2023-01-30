﻿namespace ChatApp.Api.Profiles;

public class UsersProfile : Profile
{
	public UsersProfile()
	{
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MessageDto, Message>();
            cfg.CreateMap<GroupDto, Group>();
        });

        var mapper = new Mapper(config);
        // source -> destination
        CreateMap<AppUser, UserDto>();
        CreateMap<UserDto, AppUser>().ConstructUsing(src => 
			new AppUser() 
			{
				UserName = src.UserName,
				Email = src.Email,
				PhoneNumber = src.PhoneNumber,
				Messages = mapper.Map<ICollection<Message>>(src.Messages),
				Groups = mapper.Map<ICollection<Group>>(src.Groups),
				LastTimeOnline = src.LastTimeOnline,
			});

		CreateMap<RegisterDto, AppUser>().ConstructUsing(src =>
            new AppUser()
            {
                UserName = src.UserName,
                Email = src.Email,
                PhoneNumber = src.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                LastTimeOnline = DateTime.Now,
            });
        CreateMap<UserDto, UserWithoutEntities>();
    }
}
