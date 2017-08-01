
import com.google.gson.annotations.SerializedName;

import org.apache.commons.lang3.StringUtils;


public class Service {

    @SerializedName(MESSAGE)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mMessage;

    @SerializedName(RESULT)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mResult;


    @NonNull
    public final Result getResult() {
        if (!StringUtils.isEmpty(mResult) && StringUtils.isNumeric(mResult)) {
            final int key = Integer.parseInt(mResult);

            final Result result = Result.get(key);

            if (result != null) {
                return result;
            }
        }

        return Result.UNKNOWN;
    }

    @Nullable
    public final String getMessage() {
        return mMessage;
    }


    public enum Result {

        UNKNOWN(-1),

        TRANSACTION_FAILED(0),

        @SuppressWarnings(SPELL_CHECKING_INSPECTION)
        TRANSACTION_SUCCESFULLY(1),

        AUTHENTICATION_FAILED(2),

        STUDENT_OUTSTANDING_PAYMENTS(3),

        STUDENT_WITHOUT_REGISTRATION(4),

        STUDENT_ENROLLMENT_CLASS_NO_ACADEMIC_OFFERINGS(5);

        private static final SparseArray<Result> mSparseArray;

        static {
            mSparseArray = new SparseArray<>();

            for (final Result value : Result.values()) {
                mSparseArray.put(value.mValue, value);
            }
        }

        private final int mValue;

        Result(final int value) {
            mValue = value;
        }

        @Nullable
        private static Result get(final int key) {
            return mSparseArray.get(key);
        }

    }


    protected static class SerializedNames {

        static final String MESSAGE = "message";

        static final String RESULT = "result";

    }

}
