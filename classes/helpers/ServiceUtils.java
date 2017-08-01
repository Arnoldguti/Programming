


import org.apache.commons.lang3.StringUtils;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;


public class ServiceUtils {

    @SuppressWarnings(SPELL_CHECKING_INSPECTION)
    private static final String CONFIG_URL = "ztDZKrMvf2PmwDsOwf2TBth4A8P3+xRYwWrqaxzOjhw=";


    @Nullable
    public static ConfigurationService getConfigurationService(@NonNull final Context context) {
        final String baseUrl = CryptUtils.decrypt(context, CONFIG_URL);

        if (!StringUtils.isEmpty(baseUrl)) {
            final Retrofit.Builder builder = getRetrofitBuilder();

            return builder
                    .baseUrl(baseUrl)
                    .build()
                    .create(ConfigurationService.class);
        }

        return null;
    }

    @Nullable
    public static <T> T getService(@NonNull final Context context,
                                   @NonNull final Class<T> service) {
        final Retrofit retrofit = getRetrofit(context);

        if (retrofit != null) {
            return retrofit.create(service);
        }

        return null;
    }


    @Nullable
    private static Retrofit getRetrofit(@NonNull final Context context) {
        final String baseUrl = Preferences.Services
                .getValue(context, BASE_URL);

        if (!StringUtils.isEmpty(baseUrl)) {
            final Retrofit.Builder builder = getRetrofitBuilder();

            return builder
                    .baseUrl(baseUrl)
                    .build();
        }

        return null;
    }

    @NonNull
    private static Retrofit.Builder getRetrofitBuilder() {
        return new Retrofit.Builder()
                .addConverterFactory(GsonConverterFactory.create());
    }

}
