using System.Collections.Generic;

namespace Library.Client.Net
{
    public class RecordingClientManager
    {
        public Dictionary<int, CommonClient> recordingClients;
        public RecordingClient client;
        public RecordingClientManager()
        {
            recordingClients = new Dictionary<int, CommonClient>();
        }

        public void Add(CommonClient sender)
        {
            client = (RecordingClient)sender;
            if (!recordingClients.ContainsKey(client.devSerial))
            {
                recordingClients.Add(client.devSerial, client);
            }
        }

        public void Remove(CommonClient sender)
        {
            client = (RecordingClient)sender;
            if (recordingClients.ContainsKey(client.devSerial))
            {
                recordingClients.Remove(client.devSerial);
            }
        }
    }
}
