using Evolution.ValidationService.Interfaces;
using ManateeJson = Manatee.Json;
using ManateeSchema = Manatee.Json.Schema;

namespace Evolution.ValidationService.Services
{
    public class ValidationService : IValidationService
    {
        public void Validate(string modelJson,
                             string filePath,
                             ref ManateeSchema.SchemaValidationResults results)

        {
            var json = ManateeJson.JsonValue.Parse(modelJson);
            //var schemaText = File.ReadAllText(filePath);
            //var schemaJson = ManateeJson.JsonValue.Parse(schemaText);
            //var schema = new JsonSerializer().Deserialize<ManateeSchema.JsonSchema>(schemaJson);


            var schema = ManateeSchema.JsonSchemaRegistry.Get(filePath);
            results = schema.Validate(json);
        }

    }
}
