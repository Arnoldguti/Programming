
import com.google.gson.annotations.SerializedName;
import com.uth.uth.R;
import com.uth.uth.api.model.clazz.enrollment.Class;

import org.apache.commons.lang3.StringUtils;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

import static com.uth.uth.api.model.clazz.Schedule.SerializedNames.AVAILABILITY;
import static com.uth.uth.api.model.clazz.Schedule.SerializedNames.CAMPUS_CODE;
import static com.uth.uth.api.model.clazz.Schedule.SerializedNames.CLASSROOM;
import static com.uth.uth.api.model.clazz.Schedule.SerializedNames.DAYS;
import static com.uth.uth.api.model.clazz.Schedule.SerializedNames.END_TIME;
import static com.uth.uth.api.model.clazz.Schedule.SerializedNames.GROUP;
import static com.uth.uth.api.model.clazz.Schedule.SerializedNames.IDENTIFIER;
import static com.uth.uth.api.model.clazz.Schedule.SerializedNames.START_TIME;
import static com.uth.uth.api.model.clazz.enrollment.Campus.SerializedNames.CODE;
import static com.uth.uth.util.Constants.Annotation.UNUSED_DECLARATION;

public class Schedule extends Class {

    private static final String DAYS_REGEX = "-";


    @SerializedName(AVAILABILITY)
    @SuppressWarnings(UNUSED_DECLARATION)
    private int mAvailability;

    @SerializedName(GROUP)
    @SuppressWarnings(UNUSED_DECLARATION)
    private int mGroup;


    @SerializedName(CODE)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mCampus;

    @SerializedName(CAMPUS_CODE)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mCampusCode;

    @SerializedName(CLASSROOM)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mClassroom;

    @SerializedName(DAYS)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mDays;

    @SerializedName(END_TIME)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mEndTime;

    @SerializedName(IDENTIFIER)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mIdentifier;

    @SerializedName(START_TIME)
    @SuppressWarnings(UNUSED_DECLARATION)
    private String mStartTime;


    public final int getAvailability() {
        return mAvailability;
    }

    public final int getGroup() {
        return mGroup;
    }


    @Nullable
    public final ArrayList<Day> getDays() {
        if (!StringUtils.isEmpty(mDays)) {
            final ArrayList<Day> days = new ArrayList<>();

            for (final String day : mDays.split(DAYS_REGEX)) {
                days.add(Day.get(day));
            }

            return days;
        }

        return null;
    }

    @Nullable
    public final String getCampus() {
        return mCampus;
    }

    @Nullable
    public final String getCampusCode() {
        return mCampusCode;
    }

    @Nullable
    public final String getClassroom() {
        return mClassroom;
    }

    @Nullable
    public final String getEndTime() {
        return mEndTime;
    }

    @Nullable
    public final String getIdentifier() {
        return mIdentifier;
    }

    @Nullable
    public final String getStartTime() {
        return mStartTime;
    }


    public enum Day {

        SUNDAY("DO", R.string.title_weekday_sunday),

        MONDAY("LU", R.string.title_weekday_monday),

        TUESDAY("MA", R.string.title_weekday_tuesday),

        WEDNESDAY("MI", R.string.title_weekday_wednesday),

        THURSDAY("JU", R.string.title_weekday_thursday),

        FRIDAY("VI", R.string.title_weekday_friday),

        SATURDAY("SA", R.string.title_weekday_saturday);

        private static final Map<String, Day> mMap;

        static {
            mMap = new HashMap<>();

            for (final Day day : Day.values()) {
                mMap.put(day.mDay, day);
            }
        }

        @StringRes
        private final int mStringResId;

        private final String mDay;

        Day(@NonNull final String day, @StringRes final int stringRes) {
            mDay = day;

            mStringResId = stringRes;
        }

        @Nullable
        private static Day get(@NonNull final String day) {
            return mMap.get(day);
        }

        @StringRes
        public final int getStringResId() {
            return mStringResId;
        }

    }


    public static class SerializedNames {

        public static final String GROUP = "group";

        public static final String IDENTIFIER = "identifier";


        static final String AVAILABILITY = "availability";

        static final String CAMPUS_CODE = "campus_code";

        static final String CLASSROOM = "classroom";

        static final String DAYS = "days";

        static final String END_TIME = "end_time";

        static final String START_TIME = "start_time";

    }

}
