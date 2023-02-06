using System.Collections.Generic;

namespace LambdaTriggerByS3Change.Models
{
    public class FatherClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<ChildrenClass> Children { get; set; }
    }
}
