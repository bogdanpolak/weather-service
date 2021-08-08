namespace WeatherService.Domain.Model
{
    public class TemperatureRange
    {
        public int Minimal { get; }
        public int Maximum { get; }
        public TemperatureRange(int minimal, int maximum)
        {
            Minimal = minimal;
            Maximum = maximum;
        }
    }
}