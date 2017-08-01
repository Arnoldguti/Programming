
import com.google.gson.annotations.SerializedName;
import com.yalantis.ucrop.util.BitmapLoadUtils;

import org.apache.commons.lang3.StringUtils;
import org.apache.commons.lang3.exception.ExceptionUtils;

import java.io.ByteArrayOutputStream;

public class ProfilePhoto extends Service {

    private static final String TAG = ProfilePhoto.class.getSimpleName();


    @SerializedName(IMAGE_REPRESENTATION)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mImageRepresentation;

    @Nullable
    public final Bitmap getPhoto() {
        if (!StringUtils.isEmpty(mImageRepresentation)) {
            return Utils.getDecoded(mImageRepresentation);
        }

        return null;
    }


    public static class SerializedNames {

        public static final String IMAGE_REPRESENTATION = "image_representation";

        public static final String THUMBNAIL = "thumbnail";

    }

    public static class Utils {

        @Nullable
        public static Bitmap getDecoded(@NonNull final String encoded) {
            try {
                final String input = encoded.replace(StringUtils.SPACE, "+");

                final byte[] decode = Base64.decode(input, Base64.DEFAULT);

                return BitmapFactory.decodeByteArray(decode, 0, decode.length);
            } catch (Exception ex) {
                Log.e(TAG, ExceptionUtils.getStackTrace(ex));
            }

            return null;
        }

        @Nullable
        public static String getEncoded(@NonNull final Bitmap bitmap) {
            final ByteArrayOutputStream stream = new ByteArrayOutputStream();

            try {
                final byte[] input = stream.toByteArray();

                return Base64.encodeToString(input, Base64.DEFAULT);
            } catch (Exception ex) {
                Log.e(TAG, ExceptionUtils.getStackTrace(ex));
            } finally {
                bitmap.recycle();

                BitmapLoadUtils.close(stream);
            }

            return null;
        }

    }

}
