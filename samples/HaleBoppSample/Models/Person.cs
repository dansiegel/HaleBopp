using Comet;

namespace HaleBoppSample.Models
{
    public class Person : BindingObject
    {
        public string FirstName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public string LastName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
    }
}
