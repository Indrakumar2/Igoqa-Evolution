using ManateeSchema = Manatee.Json.Schema;

namespace Evolution.ValidationService.Interfaces
{
    public interface IValidationService
    {
        void Validate(string modelJson,
                      string filePath,
                      ref ManateeSchema.SchemaValidationResults results);
    }
}
