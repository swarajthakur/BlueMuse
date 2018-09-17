﻿using BlueMuse.Helpers;
using BlueMuse.Misc;
using BlueMuse.MuseManagement;
using System;
using System.Linq;

namespace BlueMuse.Settings
{
    public sealed class AppSettings : ObservableObject
    {

        private static readonly Lazy<AppSettings> lazy =
        new Lazy<AppSettings>(() => new AppSettings());

        public static AppSettings Instance { get { return lazy.Value; } }
        Windows.Storage.ApplicationDataContainer roamingSettings;

        private AppSettings()
        {
            roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
        }

        public void LoadInitialSettings()
        {
            // Init primary timestamp format, defaults to bluemuse.
            var tf = roamingSettings.Values[Constants.SETTINGS_KEY_TIMESTAMP_FORMAT] as string;
            var tff = TimestampFormatsContainer.TimestampFormats.FirstOrDefault(x => x.Key == tf);
            TimestampFormat = tff ?? TimestampFormatsContainer.TimestampFormats.First(x => x.Key == Constants.TIMESTAMP_FORMAT_BLUEMUSE_UNIX);

            // Init secondary timestamp format, defaults to none.
            var tf2 = roamingSettings.Values[Constants.SETTINGS_KEY_TIMESTAMP_FORMAT2] as string;
            var tff2 = TimestampFormatsContainer.TimestampFormats2.FirstOrDefault(x => x.Key == tf2);
            TimestampFormat2 = tff2 ?? TimestampFormatsContainer.TimestampFormats2.First(x => x.Key == Constants.TIMESTAMP_FORMAT_NONE);
        }

        private BaseTimestampFormat timestampFormat;
        public BaseTimestampFormat TimestampFormat
        {
            get
            {
                return timestampFormat;
            }
            set
            {
                Muse.TimestampFormat = value;
                roamingSettings.Values[Constants.SETTINGS_KEY_TIMESTAMP_FORMAT] = value.Key;
                SetProperty(ref timestampFormat, value);
            }
        }
        private BaseTimestampFormat timestampFormat2;
        public BaseTimestampFormat TimestampFormat2
        {
            get
            {
                return timestampFormat2;
            }
            set
            {
                Muse.TimestampFormat2 = value;
                roamingSettings.Values[Constants.SETTINGS_KEY_TIMESTAMP_FORMAT2] = value.Key;
                SetProperty(ref timestampFormat2, value);
            }
        }
    }
}