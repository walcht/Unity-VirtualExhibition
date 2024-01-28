namespace JSONContainer
{
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ExhibitionDataContainer
    {
        [JsonProperty("_id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("event_name", Required = Required.Always)]
        public string EventName { get; set; }

        [JsonProperty("hall_type", Required = Required.Always)]
        public string HallType { get; set; }

        [JsonProperty("display_screen", Required = Required.Always)]
        public DisplayScreen DisplayScreen { get; set; }

        [JsonProperty("sponsor_disc", Required = Required.Always)]
        public SponsorDisc SponsorDisc { get; set; }

        [JsonProperty("sponsor_cylinder", Required = Required.Always)]
        public SponsorCylinder SponsorCylinder { get; set; }

        [JsonProperty("sponsor_banners", Required = Required.Always)]
        public ExhibitionSponsorBanners SponsorBanners { get; set; }

        [JsonProperty("stands", Required = Required.Always)]
        public List<StandDataContainer> Stands { get; set; }
    }

    public partial class DisplayScreen
    {
        [JsonProperty("purchased", Required = Required.Always)]
        public bool Purchased { get; set; }

        [JsonProperty("media_download_url", Required = Required.Default)]
        public string MediaDownloadUrl; // CURRENTLY THIS MEDIA IS SIMPLY A TEXTURE
    }

    public partial class SponsorDisc // VERIFIED
    {
        [JsonProperty("purchased", Required = Required.Always)]
        public bool Purchased { get; set; }

        [JsonProperty("texture_download_url", Required = Required.Always)]
        public string TextureDownloadUrl { get; set; }
    }

    public partial class SponsorCylinder // VERIFIED
    {
        [JsonProperty("purchased", Required = Required.Always)]
        public bool Purchased { get; set; }

        [JsonProperty("texture_download_url_0", Required = Required.Always)]
        public string TextureDownloadUrl0 { get; set; }

        [JsonProperty("texture_download_url_1", Required = Required.Always)]
        public string TextureDownloadUrl1 { get; set; }

        [JsonProperty("texture_download_url_2", Required = Required.Always)]
        public string TextureDownloadUrl2 { get; set; }

        [JsonProperty("texture_download_url_3", Required = Required.Always)]
        public string TextureDownloadUrl3 { get; set; }
    }

    public partial class SponsorBanner
    {
        [JsonProperty("purchased", Required = Required.Always)]
        public bool Purchased { get; set; }

        [JsonProperty("texture_download_url_0", Required = Required.Always)]
        public string TextureDownloadUrl0 { get; set; }

        [JsonProperty("texture_download_url_1", Required = Required.Always)]
        public string TextureDownloadUrl1 { get; set; }

        [JsonProperty("texture_download_url_2", Required = Required.Always)]
        public string TextureDownloadUrl2 { get; set; }

        [JsonProperty("texture_download_url_3", Required = Required.Always)]
        public string TextureDownloadUrl3 { get; set; }
    }

    public partial class StandDataContainer // VERIFIED
    {
        [JsonProperty("stand_name", Required = Required.Always)]
        public string StandName { get; set; } // VERIFIED

        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; } // VERIFIED

        [JsonProperty("position", Required = Required.Always)]
        public string Position { get; set; } // VERIFIED

        [JsonProperty("tv", Required = Required.Always)]
        public Tv Tv { get; set; } // VERIFIED

        [JsonProperty("furniture", Required = Required.Always)]
        public Furniture Furniture { get; set; } // VERIFIED

        [JsonProperty("banner", Required = Required.Always)]
        public Banner Banner { get; set; } // VERIFIED

        [JsonProperty("_id", Required = Required.Always)]
        public string Id { get; set; } // VERIFIED

        [JsonProperty("background_color", Required = Required.Default)]
        public List<long> BackgroundColor { get; set; } // VERIFIED

        [JsonProperty("logo_download_url", Required = Required.Default)]
        public string LogoDownloadUrl { get; set; } // VERIFIED

        [JsonProperty("menu", Required = Required.Default)]
        public Menu Menu { get; set; } // VERIFIED

        [JsonProperty("texture_download_url", Required = Required.Always)]
        public string TextureDownloadUrl { get; set; } // VERIFIED

        [JsonProperty("caracter_type_00", Required = Required.Always)]
        public int CaracterType00 { get; set; } // VERIFIED

        [JsonProperty("caracter_type_01", Required = Required.Default)]
        public int CaracterType01 { get; set; } // VERIFIED
    }

    public partial class Tv // VERIFIED
    {
        [JsonProperty("purchased", Required = Required.Always)]
        public bool Purchased { get; set; }

        [JsonProperty("media_download_url", Required = Required.Default)]
        public string MediaDownloadUrl; // CURRENTLY THIS MEDIA IS SIMPLY A TEXTURE
    }

    public partial class Furniture // VERIFIED
    {
        [JsonProperty("purchased", Required = Required.Always)]
        public bool Purchased { get; set; }

        [JsonProperty("furniture_type", Required = Required.Always)]
        public int FurnitureType { get; set; }

        [JsonProperty("color", Required = Required.Default)]
        public List<long> Color { get; set; }
    }

    public partial class Banner // VERIFIED
    {
        [JsonProperty("purchased", Required = Required.Always)]
        public bool Purchased { get; set; }

        [JsonProperty("banner_type", Required = Required.Always)]
        public int BannerType { get; set; }

        [JsonProperty("texture_download_url", Required = Required.Always)]
        public string TextureDownloadUrl { get; set; }
    }

    public partial class Menu // VERIFIED
    {
        [JsonProperty("meet_link", Required = Required.Default)]
        public string MeetLink { get; set; } // VERIFIED

        [JsonProperty("website", Required = Required.Default)]
        public string Website { get; set; } // VERIFIED

        [JsonProperty("phoneNumber", Required = Required.Default)]
        public string PhoneNumber { get; set; } // VERIFIED

        [JsonProperty("address", Required = Required.Default)]
        public string Address { get; set; } // VERIFIED

        [JsonProperty("description", Required = Required.Default)]
        public string Description { get; set; } // VERIFIED

        [JsonProperty("pdf_download_url", Required = Required.Default)]
        public string PdfDownloadUrl { get; set; } // VERIFIED
    }

    public partial class WebinarDataContainer
    {
        [JsonProperty("data", Required = Required.Default)]
        public WebinarVideosContainer Data { get; set; }
    }

    public partial class WebinarVideosContainer // VERIFIED
    {
        [JsonProperty("purchased", Required = Required.Always)]
        public bool Purchased { get; set; } // VERIFIED

        [JsonProperty("videos", Required = Required.Default)]
        public List<WebinarVideo> Videos { get; set; }
    }

    public partial class WebinarVideo
    {
        [JsonProperty("video_title", Required = Required.Always)]
        public string VideoTitle { get; set; }

        [JsonProperty("video_download_url", Required = Required.Always)]
        public string VideoDownloadUrl { get; set; }

        [JsonProperty("thumbnail_download_url", Required = Required.AllowNull)]
        public string ThumbnailDownloadUrl { get; set; }

        [JsonProperty("video_description", Required = Required.Always)]
        public string VideoDescription { get; set; }
    }

    public partial class EntranceDataContainer
    {
        [JsonProperty("webinar", Required = Required.Always)]
        public bool Webinar { get; set; } // VERIFIED

        [JsonProperty("sponsor_banners", Required = Required.Always)]
        public EntranceSponsorBanners SponsorBanners { get; set; } // VERIFIED

        [JsonProperty("cube_screen", Required = Required.Always)]
        public CubeScreen CubeScreen { get; set; } // VERIFIED
    }

    public partial class ExhibitionSponsorBanners // VERIFIED
    {
        [JsonProperty("purchased", Required = Required.Always)]
        public bool Purchased { get; set; }

        [JsonProperty("texture_download_url_0", Required = Required.Default)]
        public string TextureDownloadUrl0 { get; set; }

        [JsonProperty("texture_download_url_1", Required = Required.Default)]
        public string TextureDownloadUrl1 { get; set; }

        [JsonProperty("texture_download_url_2", Required = Required.Default)]
        public string TextureDownloadUrl2 { get; set; }

        [JsonProperty("texture_download_url_3", Required = Required.Default)]
        public string TextureDownloadUrl3 { get; set; }
    }

    public partial class EntranceSponsorBanners // VERIFIED
    {
        [JsonProperty("texture_download_url_0", Required = Required.Default)]
        public string TextureDownloadUrl0 { get; set; }

        [JsonProperty("texture_download_url_1", Required = Required.Default)]
        public string TextureDownloadUrl1 { get; set; }
    }

    public partial class CubeScreen // VERIFIED
    {
        [JsonProperty("texture_download_url", Required = Required.Default)]
        public string TextureDownloadUrl { get; set; }
    }

    public partial class MenuRelatedDataContainer
    {
        [JsonProperty("stand_name", Required = Required.Always)]
        public string StandName { get; set; } // VERIFIED

        [JsonProperty("logo", Required = Required.Default)]
        public string Logo { get; set; } // VERIFIED

        [JsonProperty("menu", Required = Required.Default)]
        public Menu Menu; // VERIFIED
    }

    public partial class NetworkingNbrPagesContainer
    {
        [JsonProperty("data", Required = Required.Always)]
        public int Data;
    }

    public partial class NetworkingPageDataContainer
    {
        [JsonProperty("data", Required = Required.Always)]
        public List<Visitor> Data;
    }

    public partial class Visitor
    {
        [JsonProperty("email", Required = Required.Default)]
        public string Email;

        [JsonProperty("phoneNumber", Required = Required.Default)]
        public string PhoneNumber;

        [JsonProperty("firstName", Required = Required.Default)]
        public string FirstName;

        [JsonProperty("lastName", Required = Required.Default)]
        public string LastName;

        [JsonProperty("profession", Required = Required.Default)]
        public string Profession;

        [JsonProperty("establishment", Required = Required.Default)]
        public string Establishment;
    }

    public partial class ExhibitionDataContainer
    {
        public static ExhibitionDataContainer FromJson(string json) =>
            JsonConvert.DeserializeObject<ExhibitionDataContainer>(
                json,
                JSONContainer.Converter.Settings
            );
    }

    public partial class EntranceDataContainer
    {
        public static EntranceDataContainer FromJson(string json) =>
            JsonConvert.DeserializeObject<EntranceDataContainer>(
                json,
                JSONContainer.Converter.Settings
            );
    }

    public partial class MenuRelatedDataContainer
    {
        public static MenuRelatedDataContainer FromJson(string json) =>
            JsonConvert.DeserializeObject<MenuRelatedDataContainer>(
                json,
                JSONContainer.Converter.Settings
            );
    }

    public partial class StandDataContainer
    {
        public static StandDataContainer FromJson(string json) =>
            JsonConvert.DeserializeObject<StandDataContainer>(
                json,
                JSONContainer.Converter.Settings
            );
    }

    public partial class NetworkingNbrPagesContainer
    {
        public static NetworkingNbrPagesContainer FromJson(string json) =>
            JsonConvert.DeserializeObject<NetworkingNbrPagesContainer>(
                json,
                JSONContainer.Converter.Settings
            );
    }

    public partial class NetworkingPageDataContainer
    {
        public static NetworkingPageDataContainer FromJson(string json) =>
            JsonConvert.DeserializeObject<NetworkingPageDataContainer>(
                json,
                JSONContainer.Converter.Settings
            );
    }

    public partial class WebinarDataContainer
    {
        public static WebinarDataContainer FromJson(string json) =>
            JsonConvert.DeserializeObject<WebinarDataContainer>(
                json,
                JSONContainer.Converter.Settings
            );
    }

    public static class Serialize
    {
        public static string ToJson(this ExhibitionDataContainer self) =>
            JsonConvert.SerializeObject(self, JSONContainer.Converter.Settings);

        public static string ToJson(this EntranceDataContainer self) =>
            JsonConvert.SerializeObject(self, JSONContainer.Converter.Settings);

        public static string ToJson(this MenuRelatedDataContainer self) =>
            JsonConvert.SerializeObject(self, JSONContainer.Converter.Settings);

        public static string ToJson(this StandDataContainer self) =>
            JsonConvert.SerializeObject(self, JSONContainer.Converter.Settings);

        public static string ToJson(this NetworkingNbrPagesContainer self) =>
            JsonConvert.SerializeObject(self, JSONContainer.Converter.Settings);

        public static string ToJson(this NetworkingPageDataContainer self) =>
            JsonConvert.SerializeObject(self, JSONContainer.Converter.Settings);

        public static string ToJson(this WebinarDataContainer self) =>
            JsonConvert.SerializeObject(self, JSONContainer.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
