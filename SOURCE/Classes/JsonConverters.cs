using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Launcher8.Classes {
    internal class LauncherButtonJsonConverter : JsonConverter<LauncherButton> {
        public override LauncherButton Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");
            LauncherButton button = new();
            while (reader.Read()) {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return button;
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("JsonTokenType was not PropertyName");
                var propertyName = reader.GetString();
                if (string.IsNullOrWhiteSpace(propertyName))
                    throw new JsonException("Failed to get property name");
                reader.Read();
                switch (propertyName) {
                    case "Caption":
                        button.Caption = reader.GetString() ?? "";
                        break;
                    case "Path":
                        button.Path = reader.GetString() ?? "";
                        break;
                    case "Arguments":
                        button.Arguments = reader.GetString() ?? "";
                        break;
                    case "GridLocation":
                        string? loc = reader.GetString();
                        if (string.IsNullOrWhiteSpace(loc))
                            button.GridLocation = new Point(0, 0);
                        else {
                            string?[] coords = loc.Split(',');
                            button.GridLocation = new Point(int.Parse(coords[0]!.Trim()), int.Parse(coords[1]!.Trim()));
                        }
                        break;
                    case "AdminOnly":
                        button.AdminOnly = reader.GetBoolean();
                        break;
                    case "Background":
                        string? color = reader.GetString();
                        if (string.IsNullOrWhiteSpace(color))
                            break;
                        string[] rgb = color.Split(',');
                        if (rgb.Length == 1)
                            try {
                                button.Background = Color.FromName(color);
                                break;
                            } catch {
                                break;
                            }
                        else if (rgb.Length == 3)
                            try {
                                button.Background = Color.FromArgb(int.Parse(rgb[0].Trim()), int.Parse(rgb[1].Trim()), int.Parse(rgb[2].Trim()));
                                break;
                            } catch {
                                break;
                            }
                        else if (rgb.Length == 4)
                            try {
                                button.Background = Color.FromArgb(int.Parse(rgb[0].Trim()), int.Parse(rgb[1].Trim()), int.Parse(rgb[2].Trim()), int.Parse(rgb[3].Trim()));
                                break;
                            } catch {
                                break;
                            }
                        break;
                    case "ReferenceType":
                        button.ReferenceType = (LauncherButton.RefType)reader.GetUInt16();
                        break;
                    case "TargetBrowser":
                        button.TargetBrowser = (LauncherButton.Browser)reader.GetUInt16();
                        break;
                    case "HasHotKeySet":
                        button.HasHotKeySet = reader.GetBoolean();
                        break;
                    case "KeyModifiers":
                        button.KeyModifiers = (Classes.ModifierKeys)reader.GetUInt16();
                        break;
                    case "KeyTarget":
                        button.KeyTarget = (Classes.Key)reader.GetUInt32();
                        break;
                    default:
                        break;
                }
            }
            return button;
        }
        public override void Write(
            Utf8JsonWriter writer,
            LauncherButton button,
            JsonSerializerOptions options) {
            if (button is null)
                writer.WriteNullValue();
            else {
                writer.WriteStartObject();
                writer.WritePropertyName("Caption");
                writer.WriteStringValue(button.Caption ?? "");
                writer.WritePropertyName("Path");
                writer.WriteStringValue(button.Path ?? "");
                writer.WritePropertyName("Arguments");
                writer.WriteStringValue(button.Arguments ?? "");
                writer.WritePropertyName("GridLocation");
                writer.WriteStringValue($"{button.GridLocation.X}, {button.GridLocation.Y}");
                writer.WritePropertyName("AdminOnly");
                writer.WriteBooleanValue(button.AdminOnly);
                writer.WritePropertyName("Background");
                writer.WriteStringValue(ColorToString(button.Background));
                writer.WritePropertyName("ReferenceType");
                writer.WriteNumberValue((int)button.ReferenceType);
                writer.WritePropertyName("TargetBrowser");
                writer.WriteNumberValue((int)button.TargetBrowser);
                writer.WritePropertyName("HasHotKeySet");
                writer.WriteBooleanValue(button.HasHotKeySet);
                writer.WritePropertyName("KeyModifiers");
                writer.WriteNumberValue((int)button.KeyModifiers);
                writer.WritePropertyName("KeyTarget");
                writer.WriteNumberValue((int)button.KeyTarget);
                writer.WriteEndObject();
            }
        }
        private string ColorToString(Color color) {
            if (color.IsNamedColor)
                return color.Name;
            return $"{color.R}, {color.G}, {color.B}";
        }
    }
    internal class ButtonCollectionJsonConverter : JsonConverter<ButtonCollection> {
        public override ButtonCollection Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) {
            ButtonCollection buttons = new ButtonCollection();
            JsonSerializerOptions _options = new() {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                Converters = { new LauncherButtonJsonConverter() }
            };
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected a JSON array");

            while (reader.Read()) {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;
                var button = JsonSerializer.Deserialize<LauncherButton>(ref reader, options);
                if (button is not null)
                    buttons.Add(button);
            }
            return buttons;
        }
        public override void Write(
            Utf8JsonWriter writer,
            ButtonCollection buttons,
            JsonSerializerOptions options) {
            writer.WriteStartArray();
            foreach(var button in buttons) {
                JsonSerializer.Serialize(writer, button, options);
            }
            writer.WriteEndArray();
        }
    }
}
