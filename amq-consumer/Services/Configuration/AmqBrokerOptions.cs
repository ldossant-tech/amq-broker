namespace Services.Configuration
{
    public class AmqBrokerOptions
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Queue { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // 👇 ADICIONE ISSO
        public string ExtraParams { get; set; } = string.Empty;

        // 👇 BrokerUri final (usado pelo consumer)
        public string BrokerUri =>
            $"tcp://{Host}:{Port}{ExtraParams}";
    }
}
