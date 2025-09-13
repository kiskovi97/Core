namespace Kiskovi.Core
{
    public class SettingsData : ICopyableData<SettingsData>
    {
        public float musicVolume = 1f;
        public float soundVolume = 1f;

        public SettingsData Copy()
        {
            return new SettingsData()
            {
                musicVolume = musicVolume,
                soundVolume = soundVolume
            };
        }
    }

    public interface ISettingsTable
    {
        float MusicVolume { get; set; }
        float SoundVolume { get; set; }
    }

    internal class SettingsTable : LocalDatabaseTable<SettingsData>, ISettingsTable
    {
        public float MusicVolume { get => Data.musicVolume; set => Data.musicVolume = value; }
        public float SoundVolume { get => Data.soundVolume; set => Data.soundVolume = value; }

        public SettingsTable(ISaveSystem saveSystem) : base(saveSystem)
        { }
    }
}
