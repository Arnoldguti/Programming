



import org.apache.commons.lang3.StringUtils;

import java.util.HashMap;
import java.util.Map;


public class Fee extends Service {

    @SerializedName(CHECK_AVAILABLE)
    @SuppressWarnings(UNUSED_DECLARATION)
    private boolean mCheckAvailable;

    @SerializedName(SELECTED)
    @SuppressWarnings(UNUSED_DECLARATION)
    private boolean mSelected;

    @SerializedName(UNCHECK_AVAILABLE)
    @SuppressWarnings({
            SPELL_CHECKING_INSPECTION, UNUSED_DECLARATION
    })
    private boolean mUnCheckAvailable;


    @SerializedName(DUE)
    @SuppressWarnings(UNUSED_DECLARATION)
    private float mDue;

    @SerializedName(ENROLLMENT)
    @SuppressWarnings(UNUSED_DECLARATION)
    private float mEnrollment;

    @SerializedName(TOTAL_PAY)
    @SuppressWarnings(UNUSED_DECLARATION)
    private float mTotalPay;


    @SerializedName(NUMBER)
    @SuppressWarnings(UNUSED_DECLARATION)
    private int mNumber;


    @SerializedName(DATE_DEADLINE)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mDateDeadline;

    @SerializedName(DATE_PAID)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mDatePaid;

    @SerializedName(ID_FEE)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mIdFee;

    @SerializedName(STATUS)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mStatus;


    public final boolean isCheckAvailable() {
        return mCheckAvailable;
    }

    public final boolean isSelected() {
        return mSelected;
    }

    public final boolean isUnCheckAvailable() {
        return mUnCheckAvailable;
    }


    public final float getDue() {
        return mDue;
    }

    public final float getEnrollment() {
        return mEnrollment;
    }

    public final float getTotalPay() {
        return mTotalPay;
    }


    public final int getNumber() {
        return mNumber;
    }


    @NonNull
    public final Status getStatus() {
        if (!StringUtils.isEmpty(mStatus)) {
            final Status status = Status.get(mStatus);

            if (status != null) {
                return status;
            }
        }

        return Status.UNKNOWN;
    }

    @Nullable
    public final String getDateDeadline() {
        return mDateDeadline;
    }

    @Nullable
    public final String getDatePaid() {
        return mDatePaid;
    }

    @Nullable
    public final String getIdFee() {
        return mIdFee;
    }


    public enum Status {

        UNKNOWN(null, 0),

        ACTIVE("A", R.string.title_student_billing_fee_status_active),

        INACTIVE("N", R.string.title_student_billing_fee_status_inactive),

        PAID_OUT("C", R.string.title_student_billing_fee_status_pay_out);

        private static final Map<String, Status> mMap;

        static {
            mMap = new HashMap<>();

            for (final Status status : Status.values()) {
                mMap.put(status.mValue, status);
            }
        }

        @StringRes
        private final int mStringResId;

        private final String mValue;

        Status(@Nullable final String value, @StringRes final int stringResId) {
            mValue = value;

            mStringResId = stringResId;
        }

        @Nullable
        private static Status get(@NonNull final String status) {
            return mMap.get(status);
        }

        @StringRes
        public final int getStringResId() {
            return mStringResId;
        }

    }


    public static class SerializedNames {

        public static final String ID_FEE = "id_fee";

        public static final String NUMBER = "number";

        public static final String SELECTED = "selected";


        static final String CHECK_AVAILABLE = "check_available";

        static final String DATE_DEADLINE = "date_deadline";

        static final String DATE_PAID = "date_paid";

        static final String DUE = "due";

        static final String ENROLLMENT = "enrollment";

        static final String TOTAL_PAY = "total_pay";

        @SuppressWarnings(SPELL_CHECKING_INSPECTION)
        static final String UNCHECK_AVAILABLE = "uncheck_available";

    }

}
