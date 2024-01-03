using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReinforcementLearning;
using System.Reflection;

namespace TestProject.Training
{
    [TestClass]
    public class NfqTest
    {
        private FieldInfo GetPrivateField<T>(object obj, string fieldName)
        {
            var type = typeof(T);
            return type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private T GetPrivateFieldValue<T>(object obj, string fieldName)
        {
            FieldInfo field = GetPrivateField<NeuralNetwork>(obj, fieldName);
            return (T)field.GetValue(obj);
        }
    }
}
