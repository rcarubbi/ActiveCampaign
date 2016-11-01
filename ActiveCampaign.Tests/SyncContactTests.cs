using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActiveCampaign.Tests
{
    [TestClass]
    public class SyncContactTests
    {
        [TestMethod]
        public void Sync_Contact_To_List_With_Success()
        {
            var contact = GivenValidContact();
            var result = WhenApiCalled(contact);
            ShouldReturnSucces(result);
        }

        private void ShouldReturnSucces(BaseOutput result)
        {
            Assert.IsTrue(result.Success);
        }

        private BaseOutput WhenApiCalled(SyncContactInput input)
        {
            ActiveCampaign ac = new ActiveCampaign("<YOUR_URL>", "<YOUR_API_KEY>");
            return ac.SyncContact(input);
        }

        private SyncContactInput GivenValidContact()
        {
            var input = new SyncContactInput
            {
                Email = "email@email",
                FirstName = "Name",
                LastName = "LastName",
                ListId = "1",
                Tags = "Test,Test2"
            };

            input.CustomFields.Add(new CustomField
            {
                Id = 1,
                Value = "xx"
            });


            return input;
        }
    }
}
