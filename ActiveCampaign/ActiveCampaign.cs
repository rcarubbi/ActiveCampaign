using System.Linq;
using System.Text;

namespace ActiveCampaign
{
    public class ActiveCampaign : Connector
    {
        public ActiveCampaign(string apiUrl, string apikey)
            : base(apiUrl, apikey)
        {
            TestConnection();
        }

        public BaseOutput SyncContact(SyncContactInput input)
        {
            StringBuilder strParams = new StringBuilder();
            AppendParam(strParams, "email={0}", input.Email);
            AppendParam(strParams, "&first_name={0}", input.FirstName);
            AppendParam(strParams, "&last_name={0}", input.LastName);
            AppendParam(strParams, "&p[{0}]={0}", input.ListId);
            AppendParam(strParams, "&tags={0}", input.Tags);

            foreach (var item in input.CustomFields)
            {
                AppendParam(strParams, "&field[{0},0]={1}", item.Id.ToString(), item.Value);
            }

            var response = Post(ActiveCampaignMethods.contact_sync, strParams.ToString());

            BaseOutput output = new BaseOutput
            {
                Success = response.result_code == 1,
                Message = response.result_message,
                ResultOutput = response.result_output
            };

            return output;
        }

        public BaseOutput AddContact(AddContactInput input)
        {
            StringBuilder strParams = new StringBuilder();
            AppendParam(strParams, "email={0}", input.Email);
            AppendParam(strParams, "&first_name={0}", input.FirstName);
            AppendParam(strParams, "&last_name={0}", input.LastName);
            AppendParam(strParams, "&p[{0}]={0}", input.ListId);
            AppendParam(strParams, "&tags={0}", input.Tags);
           
            foreach (var item in input.CustomFields)
            {
                AppendParam(strParams, "&field[{0},0]={1}", item.Id.ToString(), item.Value);
            }

            var response = Post(ActiveCampaignMethods.contact_add, strParams.ToString());

            BaseOutput output = new BaseOutput {
                Success = response.result_code == 1,
                Message = response.result_message,
                ResultOutput = response.result_output
            };

            return output;
        }

        private void AppendParam(StringBuilder strParams, string pattern, params string[] values)
        {
            if (values.ToList().TrueForAll(x => !string.IsNullOrWhiteSpace(x)))
            {
                strParams.AppendFormat(pattern, values.Cast<object>().ToArray());
            }
        }
    }
}
