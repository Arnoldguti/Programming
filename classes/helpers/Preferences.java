om.uth.uth.util.CryptUtils;

import org.apache.commons.collections4.MapUtils;
import org.apache.commons.lang3.StringUtils;
import org.jetbrains.annotations.Contract;

import java.util.Arrays;
import java.util.List;
import java.util.Map;

public class Preferences {

    public static boolean updateConfiguration(@NonNull
                                              final Context context,
                                              @NonNull
                                              final Configuration config) {
        final Map<String, String> parameters = config.getParameters();

        if (MapUtils.isEmpty(parameters)) {
            return false;
        }

        for (final Parameters.Key key : Parameters.Key.values()) {
            final String value = parameters.get(key.getSerializedName());

            if (StringUtils.isEmpty(value)) {
                continue;
            }

            Parameters.setValue(context, key, value);
        }


        final Map<String, String> services = config.getServices();

        if (MapUtils.isEmpty(services)) {
            return false;
        }

        for (final Services.Key key : Services.Key.values()) {
            final String value = services.get(key.getSerializedName());

            if (StringUtils.isEmpty(value)) {
                continue;
            }

            final String iv = Parameters.getValue(context, Parameters.Key.KEY_IV);

            if (StringUtils.isEmpty(iv)) {
                continue;
            }

            final String decryptedValue = CryptUtils.decrypt(context, value, iv);

            if (StringUtils.isEmpty(decryptedValue)) {
                continue;
            }

            Services.setValue(context, key, decryptedValue);
        }


        return true;
    }


    private static boolean isEmpty(@NonNull final SharedPreferences prefs) {
        return prefs.getAll().isEmpty();
    }


    @NonNull
    private static SharedPreferences getSharedPreferences(@NonNull
                                                          final Context context,
                                                          @NonNull
                                                          final String prefs) {
        return context.getSharedPreferences(prefs, Context.MODE_PRIVATE);
    }


    @Nullable
    private static String getValue(@NonNull
                                   final Context context,
                                   @NonNull
                                   final SharedPreferences prefs,
                                   @NonNull
                                   final String key) {
        final String value = prefs.getString(key, null);

        if (!StringUtils.isEmpty(value)) {
            final String iv = Crypt.getRandomIV(context);

            if (StringUtils.isEmpty(iv)) {
                Crypt.validate(context);
            } else {
                return CryptUtils.decrypt(context, value, iv);
            }
        }

        return null;
    }

    private static void setValue(@NonNull
                                 final Context context,
                                 @NonNull
                                 final SharedPreferences prefs,
                                 @NonNull
                                 final String key,
                                 @NonNull
                                 final String value) {
        if (StringUtils.isEmpty(value)) {
            return;
        }

        final String iv = Crypt.getRandomIV(context);

        if (StringUtils.isEmpty(iv)) {
            return;
        }

        final String encryptedValue = CryptUtils.encrypt(context, value, iv);

        if (StringUtils.isEmpty(encryptedValue)) {
            Crypt.validate(context);
        } else {
            prefs.edit().putString(key, encryptedValue).apply();
        }
    }


    private static void clear(@NonNull final SharedPreferences prefs) {
        if (!isEmpty(prefs)) {
            prefs.edit().clear().apply();
        }
    }

    private static void remove(@NonNull
                               final SharedPreferences prefs,
                               @NonNull
                               final String key) {
        if (prefs.contains(key)) {
            prefs.edit().remove(key).apply();
        }
    }


    public static class Crypt {

        private static final String PREFS_FILENAME = "crypt_prefs";


        public static void validate(@NonNull final Context context) {
            final String iv = getRandomIV(context);

            if (StringUtils.isEmpty(iv)) {
                final List<String> prefsFilenames = Arrays
                        .asList(PREFS_FILENAME, Parameters.PREFS_FILENAME, Services
                                .PREFS_FILENAME, Session.PREFS_FILENAME);

                for (final String prefsFilename : prefsFilenames) {
                    final SharedPreferences prefs = getSharedPreferences(context, prefsFilename);

                    clear(prefs);
                }

                final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

                final String key = Key.RANDOM_IV.toString();

                final String value = CryptUtils.generateRandomIV();

                prefs.edit().putString(key, value).apply();
            }
        }


        @Nullable
        private static String getRandomIV(@NonNull final Context context) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            final String key = Key.RANDOM_IV.toString();

            return prefs.getString(key, null);
        }


        private enum Key {

            RANDOM_IV("RandomIV");

            @NonNull
            private final String mKey;

            Key(@NonNull final String key) {
                mKey = key;
            }

            @Contract(pure = true)
            @NonNull
            @Override
            public String toString() {
                return mKey;
            }

        }

    }

    public static class Services {

        private static final String PREFS_FILENAME = "services_prefs";


        public static boolean isEmpty(@NonNull final Context context) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            return Preferences.isEmpty(prefs);
        }


        @Nullable
        public static String getValue(@NonNull final Context context, @NonNull final Key key) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            return Preferences.getValue(context, prefs, key.toString());
        }

        private static void setValue(@NonNull
                                     final Context context,
                                     @NonNull
                                     final Key key,
                                     @NonNull
                                     final String value) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            Preferences.setValue(context, prefs, key.toString(), value);
        }


        public enum Key {

            BASE_URL("BaseUrl", "base_url");

            @NonNull
            private final String mKey;

            @NonNull
            private final String mSerializedName;

            Key(@NonNull final String key, @NonNull final String serializedName) {
                mKey = key;

                mSerializedName = serializedName;
            }

            @Contract(pure = true)
            @NonNull
            @Override
            public String toString() {
                return mKey;
            }

            @NonNull
            public String getSerializedName() {
                return mSerializedName;
            }

        }

    }

    public static class Session {

        private static final String PREFS_FILENAME = "session_prefs";


        @Nullable
        public static String getValue(@NonNull final Context context, @NonNull final Key key) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            return Preferences.getValue(context, prefs, key.toString());
        }

        static void setValue(@NonNull
                             final Context context,
                             @NonNull
                             final Key key,
                             @NonNull
                             final String value) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            Preferences.setValue(context, prefs, key.toString(), value);
        }


        public static void remove(@NonNull final Context context, @NonNull final Key key) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            Preferences.remove(prefs, key.toString());
        }


        public enum Key {

            ACCOUNT_NUMBER("AccountNumber"), PASSWORD("Password");

            @NonNull
            private final String mKey;

            Key(@NonNull final String key) {
                mKey = key;
            }

            @Contract(pure = true)
            @NonNull
            @Override
            public String toString() {
                return mKey;
            }

        }

    }


    static class Parameters {

        private static final String PREFS_FILENAME = "params_prefs";


        static boolean isEmpty(@NonNull final Context context) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            return Preferences.isEmpty(prefs);
        }


        static boolean getBoolean(@NonNull final Context context, @NonNull final Key key) {
            final String value = getValue(context, key);

            return !StringUtils.isEmpty(value) && Boolean.parseBoolean(value);
        }


        @Nullable
        static String getValue(@NonNull final Context context, final @NonNull Key key) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            return Preferences.getValue(context, prefs, key.toString());
        }

        private static void setValue(@NonNull
                                     final Context context,
                                     @NonNull
                                     final Key key,
                                     @NonNull
                                     final String value) {
            final SharedPreferences prefs = getSharedPreferences(context, PREFS_FILENAME);

            Preferences.setValue(context, prefs, key.toString(), value);
        }


        enum Key {

            AUTO_UPDATE("AutoUpdate", "auto_update"),

            MAINTENANCE_MODE("MaintenanceMode", "maintenance_mode"),

            KEY_IV("KeyIV", "key_iv"),

            LATEST_VERSION("LatestVersion", "latest_version"),

            MIN_VERSION("MinVersion", "min_version");

            @NonNull
            private final String mKey;

            @NonNull
            private final String mSerializedName;

            Key(@NonNull final String key, @NonNull final String serializedName) {
                mKey = key;

                mSerializedName = serializedName;
            }

            @Contract(pure = true)
            @NonNull
            @Override
            public String toString() {
                return mKey;
            }

            @NonNull
            public String getSerializedName() {
                return mSerializedName;
            }

        }

    }

}
