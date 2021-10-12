using Mountain.Core;
using Mountain.Core.Enums;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using static Mountain.Core.DataValidation;

namespace Mountain.Mojang
{
    public static class MojangDataSource
    {
        public const string JsonContentType = "application/json";
        public const string StatusUrl = "https://status.mojang.com/check";
        public const string Profiles = "https://api.mojang.com/user/profiles/";
        public const string ProfilesMinecraft = "https://api.mojang.com/users/profiles/minecraft/";
        public const string SessionServerProfile = "https://sessionserver.mojang.com/session/minecraft/profile/";
        public const string UuidFromUsernameAtTime = "?at=";
        public const string UuidProfileNameHistory = "/names";
        public const string SessionProfileSignature = "?unsigned=false";

        public static MojangResponse<MojangStatus> GetStatus()
        {
            var data = GetJsonData(StatusUrl, out HttpStatusCode code, out Encoding charset).ToArray();
            if (code == HttpStatusCode.OK)
            {
                return new MojangResponse<MojangStatus>(FormatStatus(charset.GetChars(data)));
            }
            else
            {
                return new MojangResponse<MojangStatus>(ReadError(data), code);
            }
        }

        public static MojangStatus FormatStatus(char[] charArr)
        {
            var response = new MojangStatus();
            // Non-standard Json parser

            bool readingKey = false;
            bool readingValue = false;

            var keyBuild = new StringBuilder();
            var valueBuild = new StringBuilder();

            for (int i = 0; i < charArr.Length; i++)
            {
                if (charArr[i] == '"')
                {
                    if (readingKey)
                    {
                        readingKey = !readingValue && readingKey;
                        readingValue = true;
                    }
                    else
                    {
                        readingKey = !readingValue || readingKey;
                        readingValue = false;
                    }
                }
                else if (readingKey && !readingValue)
                {
                    keyBuild.Append(charArr[i]);
                }
                else if (!readingKey && readingValue)
                {
                    valueBuild.Append(charArr[i]);
                }
                else if (!(readingKey || readingValue) && keyBuild.Length > 0)
                {
                    if (Enum.TryParse(typeof(ColorCodeStatus), valueBuild.ToString(), true, out object value))
                    {
                        var enumValue = (ColorCodeStatus)value;
                        switch (keyBuild.ToString().ToLower())
                        {
                            case "minecraft.net":
                                response.MinecraftNet = enumValue;
                                break;
                            case "session.minecraft.net":
                                response.SessionMinecraftNet = enumValue;
                                break;
                            case "account.mojang.com":
                                response.AccountMojangCom = enumValue;
                                break;
                            case "auth.mojang.com":
                                response.AuthMojangCom = enumValue;
                                break;
                            case "skins.minecraft.net":
                                response.SkinsMinecraftNet = enumValue;
                                break;
                            case "authserver.mojang.com":
                                response.AuthServerMojangCom = enumValue;
                                break;
                            case "sessionserver.mojang.com":
                                response.SessionServerMojangCom = enumValue;
                                break;
                            case "api.mojang.com":
                                response.ApiMojangCom = enumValue;
                                break;
                            case "textures.minecraft.net":
                                response.TexturesMinecraftNet = enumValue;
                                break;
                            case "mojang.com":
                                response.MojangCom = enumValue;
                                break;
                            default:
                                break;
                        }
                    }
                    keyBuild.Clear();
                    valueBuild.Clear();
                }
            }
            return response;
        }

        public static MojangResponse<PlayerNameIdPair> GetProfileFromUsername(string username, long atUnixTime = long.MinValue)
        {
            var requestUrl = ProfilesMinecraft + ValidateUsername(username) + (atUnixTime == long.MinValue ? "" : UuidFromUsernameAtTime + atUnixTime);
            var data = GetJsonData(requestUrl, out HttpStatusCode code, out _).ToArray();
            return DeserializeTo<PlayerNameIdPair>(data, code);
        }

        public static MojangResponse<NameHistory[]> GetNameHistory(Uuid uuid)
        {
            var data = GetJsonData(Profiles + uuid.ToDehyphenatedString() + UuidProfileNameHistory, out HttpStatusCode code, out _).ToArray();
            return DeserializeTo<NameHistory[]>(data, code);
        }

        public static MojangResponse<PlayerNameIdPair[]> GetMultipleProfiles(params string[] usernames)
        {
            ValidateBounds(usernames, 1, 10);
            Array.ForEach(usernames, x => ValidateUsername(x));
            var payload = JsonSerializer.SerializeToUtf8Bytes(usernames, typeof(string[]));
            var data = PostJsonDataPayload(ProfilesMinecraft, payload, out HttpStatusCode code, out _).ToArray();

            return DeserializeTo<PlayerNameIdPair[]>(data, code);
        }

        public static MojangResponse<SessionProfile> GetSessionServerProfile(Uuid uuid, bool getSignature = false)
        {
            var data = GetJsonData(SessionServerProfile + uuid.ToDehyphenatedString() + (getSignature ? SessionProfileSignature : ""), out HttpStatusCode code, out _).ToArray();
            return DeserializeTo<SessionProfile>(data, code);
        }

        private static MojangResponse<T> DeserializeTo<T>(byte[] data, HttpStatusCode code)
        {
            if (code == HttpStatusCode.OK)
            {
                return new MojangResponse<T>(JsonSerializer.Deserialize<T>(data));
            }
            else
            {
                return new MojangResponse<T>(ReadError(data), code);
            }
        }

        private static ErrorResponse ReadError(byte[] data)
        {
            try
            {
                return JsonSerializer.Deserialize<ErrorResponse>(data);
            }
            catch
            {
                return new ErrorResponse() { Error = "Error", ErrorMessage = "Unknown Error Data" };
            }
        }

        private static MemoryStream GetJsonData(string url, out HttpStatusCode code, out Encoding charset)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Accept = JsonContentType;
            using var response = (HttpWebResponse)request.GetResponse();
            code = response.StatusCode;
            try
            {
                charset = string.IsNullOrWhiteSpace(response.ContentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(response.ContentEncoding);
            }
            catch
            {
                charset = Encoding.UTF8;
            }

            if (code == HttpStatusCode.OK)
            {
                var ms = new MemoryStream();
                response.GetResponseStream().CopyTo(ms);
                return ms;
            }
            return null;
        }

        private static MemoryStream PostJsonDataPayload(string url, byte[] payload, out HttpStatusCode code, out Encoding charset)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            request.ContentType = JsonContentType;
            request.Accept = JsonContentType;

            var requestStream = request.GetRequestStream();
            requestStream.Write(payload);
            request.ContentLength = payload.Length;

            using var response = (HttpWebResponse)request.GetResponse();
            code = response.StatusCode;
            try
            {
                charset = string.IsNullOrWhiteSpace(response.ContentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(response.ContentEncoding);
            }
            catch
            {
                charset = Encoding.UTF8;
            }

            if (code == HttpStatusCode.OK)
            {
                var ms = new MemoryStream();
                response.GetResponseStream().CopyTo(ms);
                return ms;
            }
            return null;
        }
    }
}
