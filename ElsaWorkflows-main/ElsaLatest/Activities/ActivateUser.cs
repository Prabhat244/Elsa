using System.Linq;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Samples.UserRegistration.Web.Persistence;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.Samples.UserRegistration.Web.Activities
{
    [Activity(Category = "Users", Description = "Activate a User", Outcomes = new[]{ OutcomeNames.Done, "Not Found" })] //, Icon = "fas fa-user-check"
    public class ActivateUser : Activity
    {
        private readonly SampleDbContext _store;

        public ActivateUser(SampleDbContext store)
        {
            _store = store;
        }

        [ActivityProperty(Hint = "Enter an expression that evaluates to the ID of the user to activate.")]
        public string UserId
        {
            get;
            set;
        }

        protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
        {
            var user = _store.Users.AsQueryable().Where(x => x.Id == UserId).FirstOrDefault();

            if (user == null)
                return Outcome("Not Found");

            user.IsActive = true;
            _store.Users.Update(user);
            _store.SaveChanges();
            
            return Done();
        }
    }
}