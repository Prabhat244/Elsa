using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Samples.UserRegistration.Web.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using System.Linq;

namespace Elsa.Samples.UserRegistration.Web.Activities
{
    [Activity(Category = "Users", Description = "Delete a User", Outcomes = new[]{ OutcomeNames.Done, "Not Found" })]  //, Icon = "fas fa-user-minus"
    public class DeleteUser : Activity
    {
        private readonly SampleDbContext _store;

        public DeleteUser (SampleDbContext store)
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
            var result = _store.Users.FirstOrDefault(x => x.Id == UserId);       
            _store.Users.Remove(result);
            _store.SaveChanges();

            return Done();  
        }
    }
}