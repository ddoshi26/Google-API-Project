using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary.NaturalLanguageIntelligence {
    public class ClassifyTextResponse {
        private List<ClassificationCategory> categories;

        public List<ClassificationCategory> Categories {
            get => categories;
            set => categories = value;
        }

        public ClassifyTextResponse(List<ClassificationCategory> categories) {
            Categories = categories;
        }
    }
}