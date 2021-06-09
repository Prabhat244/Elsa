using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Samples.UserRegistration.Web.Models;
using Elsa.Samples.UserRegistration.Web.Services;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.ActivityResults;
using Elsa.Samples.UserRegistration.Web.Persistence;
using Elsa.Design;
using System.Reflection;
using Elsa.Samples.UserRegistration.Web.Activities.Providers;
using Elsa.Metadata;
using System;

namespace Elsa.Samples.UserRegistration.Web.Activities
{
    [Activity(Category = "Users", Description = "Create a User", Outcomes = new[] { OutcomeNames.Done })] //, Icon = "fas fa-user-plus"
    public class CreateUser : Activity,  IActivityPropertyOptionsProvider
    {
        private readonly SampleDbContext _store;
        private readonly IIdGenerator _idGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUser(
            SampleDbContext store,
            IIdGenerator idGenerator,
            IPasswordHasher passwordHasher)
        {
            _store = store;
            _idGenerator = idGenerator;
            _passwordHasher = passwordHasher;
        }

        [ActivityProperty(Hint = "Enter an expression that evaluates to the name of the user to create.")]
        public string UserName { get; set; }

        [ActivityProperty(Hint = "Enter an expression that evaluates to the email address of the user to create.")]
        public string Email { get; set; }

        [ActivityProperty(Hint = "Enter an expression that evaluates to the password of the user to create.")]
        public string Password { get; set; }

        [ActivityProperty(
           UIHint = ActivityPropertyUIHints.Dropdown,
           OptionsProvider = typeof(CountryOptionsProvider),
           DefaultSyntax = SyntaxNames.Literal,
           SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript, SyntaxNames.Liquid }
       )]
        public string Country { get; set; }

        [ActivityProperty(
            UIHint = ActivityPropertyUIHints.MultiText,
            DefaultSyntax = SyntaxNames.Json,
            SupportedSyntaxes = new[] { SyntaxNames.Json, SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public string FavoriteLanguage { get; set; }

        [ActivityProperty(
          UIHint = ActivityPropertyUIHints.CheckList,
          OptionsProvider = typeof(CreateUser),
          DefaultSyntax = SyntaxNames.Json,
          SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public string Hobbies { get; set; }
        public object GetOptions(PropertyInfo property)
        {
            return new[] { "Games", "Travelling", "Music", "Running" };
        }
        
        [ActivityProperty(
          UIHint = ActivityPropertyUIHints.RadioList,
          Options = new[] { "Male", "Female" },
          SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )] 
        public string Gender { get; set; }
        

        [ActivityProperty(
          UIHint = ActivityPropertyUIHints.MultiLine,
          SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public string AboutMe { get; set; }


        [ActivityProperty(
          UIHint = ActivityPropertyUIHints.Checkbox,
          SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript, SyntaxNames.Liquid }
        )]
        public bool Agree { get; set; }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            var hashedPassword = _passwordHasher.HashPassword(Password);

            var user = new User
            {
                Id = _idGenerator.Generate(),
                Name = UserName,
                Email = Email,
                Password = hashedPassword.Hashed,
                PasswordSalt = hashedPassword.Salt,
                Country = Country,           
                //Hobbies = Hobbies,
                //Gender = Gender,
                AboutMe = AboutMe,
                IsActive = false
            };

            _store.Users.Add(user);

            context.SetVariable("User", user);
            return Done();
        }
    }
}