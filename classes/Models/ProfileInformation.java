
import com.google.gson.annotations.SerializedName;
import com.uth.uth.R;
import com.uth.uth.api.model.clazz.Authentication;
import com.uth.uth.api.model.clazz.Service;

import org.apache.commons.lang3.StringUtils;

import java.util.HashMap;
import java.util.Map;



public class ProfileInformation extends Service {

    @SerializedName(PHOTO_EXISTS)
    @SuppressWarnings(UNUSED_DECLARATION)
    private boolean mPhotoExists;


    @SerializedName(CUSTOMER_TYPE)
    @SuppressWarnings(UNUSED_DECLARATION)
    private int mCustomerType;

    @SerializedName(RELATIONSHIP_STATUS)
    @SuppressWarnings(UNUSED_DECLARATION)
    private int mRelationshipStatus;


    @SerializedName(ABOUT)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mAbout;

    @SerializedName(CAREER)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mCareer;

    @SerializedName(EMAIL)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mEmail;

    @SerializedName(NAME)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mName;

    @SerializedName(PHONE_NUMBER)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mPhoneNumber;

    @SerializedName(SEX)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mSex;

    @SerializedName(USERNAME_FACEBOOK)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mUsernameFacebook;

    @SerializedName(USERNAME_INSTAGRAM)
    @SuppressWarnings({
            SPELL_CHECKING_INSPECTION, UNUSED_DECLARATION
    })
    private String mUsernameInstagram;

    @SerializedName(USERNAME_LINKEDIN)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mUsernameLinkedIn;

    @SerializedName(USERNAME_TWITTER)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mUsernameTwitter;


    public final boolean isPhotoExists() {
        return mPhotoExists;
    }


    @NonNull
    public final Authentication.CustomerType getCustomerType() {
        if (mCustomerType != Authentication.CustomerType.UNKNOWN.getValue()) {
            final Authentication.CustomerType type = Authentication.CustomerType
                    .get(mCustomerType);

            if (type != null) {
                return type;
            }
        }

        return Authentication.CustomerType.UNKNOWN;
    }

    @NonNull
    public final RelationshipStatus getRelationshipStatus() {
        if (mRelationshipStatus != RelationshipStatus.UNKNOWN.getValue()) {
            final RelationshipStatus status = RelationshipStatus
                    .get(mRelationshipStatus);

            if (status != null) {
                return status;
            }
        }

        return RelationshipStatus.UNKNOWN;
    }

    @NonNull
    public final Sex getSex() {
        if (!StringUtils.isEmpty(mSex)) {
            final Sex sex = Sex.get(mSex);

            if (sex != null) {
                return sex;
            }
        }

        return Sex.UNKNOWN;
    }

    @Nullable
    public final String getAbout() {
        return mAbout;
    }

    @Nullable
    public final String getCareer() {
        return mCareer;
    }

    @Nullable
    public final String getEmail() {
        return mEmail;
    }

    @Nullable
    public final String getName() {
        return mName;
    }

    @Nullable
    public final String getPhoneNumber() {
        return mPhoneNumber;
    }

    @Nullable
    public final String getUsernameFacebook() {
        return mUsernameFacebook;
    }

    @Nullable
    @SuppressWarnings(SPELL_CHECKING_INSPECTION)
    public final String getUsernameInstagram() {
        return mUsernameInstagram;
    }

    @Nullable
    public final String getUsernameLinkedIn() {
        return mUsernameLinkedIn;
    }

    @Nullable
    public final String getUsernameTwitter() {
        return mUsernameTwitter;
    }


    public enum RelationshipStatus {

        UNKNOWN(-1, 0),

        HINT(0, R.string.title_profile_relationship_status),

        SINGLE(1, R.string.title_profile_relationship_single),

        RELATIONSHIP(2, R.string.title_profile_relationship_relationship),

        ENGAGED(3, R.string.title_profile_relationship_engaged),

        MARRIED(4, R.string.title_profile_relationship_married),

        COMPLICATED(5, R.string.title_profile_relationship_complicated),

        SEPARATED(6, R.string.title_profile_relationship_separated),

        DIVORCED(7, R.string.title_profile_relationship_divorced),

        WIDOWED(8, R.string.title_profile_relationship_widowed);

        private static final SparseArray<RelationshipStatus> mSparseArray;

        static {
            mSparseArray = new SparseArray<>();

            for (final RelationshipStatus status : RelationshipStatus.values()) {
                mSparseArray.put(status.mValue, status);
            }
        }

        @StringRes
        private final int mStringResId;

        private final int mValue;

        RelationshipStatus(final int value, @StringRes final int stringResId) {
            mValue = value;

            mStringResId = stringResId;
        }

        @Nullable
        public static RelationshipStatus get(final int value) {
            return mSparseArray.get(value);
        }

        @StringRes
        public final int getStringResId() {
            return mStringResId;
        }

        public final int getValue() {
            return mValue;
        }

    }

    public enum Sex {

        UNKNOWN(null), FEMALE("F"), MALE("M");

        private static final Map<String, Sex> mMap;

        static {
            mMap = new HashMap<>();

            for (final Sex sex : Sex.values()) {
                mMap.put(sex.mValue, sex);
            }
        }

        private final String mValue;

        Sex(@Nullable final String value) {
            mValue = value;
        }

        @Nullable
        public static Sex get(@NonNull final String sex) {
            return mMap.get(sex);
        }

    }


    public static class SerializedNames {

        public static final String ABOUT = "about";

        public static final String EMAIL = "email";

        public static final String PHONE_NUMBER = "phone_number";

        public static final String RELATIONSHIP_STATUS = "relationship_status";

        public static final String SEX = "sex";

        public static final String USERNAME_FACEBOOK = "username_facebook";

        @SuppressWarnings(SPELL_CHECKING_INSPECTION)
        public static final String USERNAME_INSTAGRAM = "username_instagram";

        @SuppressWarnings(SPELL_CHECKING_INSPECTION)
        public static final String USERNAME_LINKEDIN = "username_linkedin";

        public static final String USERNAME_TWITTER = "username_twitter";


        static final String PHOTO_EXISTS = "photo_exists";

    }

}
