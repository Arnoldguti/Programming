

import net.steamcrafted.materialiconlib.MaterialDrawableBuilder.IconValue;

import org.apache.commons.lang3.StringUtils;
import org.jetbrains.annotations.Contract;

import java.util.ArrayList;
import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;

public class ScheduleAdapter extends TimelineAdapter {

    private final OnClassListener mOnClassListener;


    public ScheduleAdapter(@NonNull
                           final Context context,
                           @NonNull
                           final List<Schedule> list,
                           @NonNull
                           final OnClassListener listener) {
        super(context, list);

        mOnClassListener = listener;
    }


    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        switch (viewType) {
            case VIEW_TYPE_ITEM: {
                final Context context = getContext();

                final View view = LayoutInflater.from(context)
                        .inflate(R.layout.item_schedule, parent, false);

                return new ViewHolders.ItemViewHolder(view);
            }

            default:
                return super.onCreateViewHolder(parent, viewType);
        }
    }

    @Override
    public void onBindViewHolder(RecyclerView.ViewHolder holder, int position) {
        super.onBindViewHolder(holder, position);

        if (BuildConfig.DEBUG) {
            assert holder != null;
        }

        if (holder == null) {
            notifyItemRemoved(position);

            return;
        }

        switch (getItemViewType(position)) {
            case VIEW_TYPE_HEADER: {
                final ViewHolders.BaseHeaderViewHolder
                        headerHolder = (ViewHolders.BaseHeaderViewHolder) holder;

                final String title = getString(R.string.title_hour);

                setText(headerHolder.titleTextView, title);


                final String description;

                switch (Session.getCustomerType()) {
                    case PROFESSOR:
                        description = getString(R.string.description_professor_schedule);

                        break;

                    case STUDENT:
                        description = getString(R.string.description_student_schedule);

                        break;

                    default:
                        description = null;

                        break;
                }

                setText(headerHolder.descriptionTextView, description);

                break;
            }

            case VIEW_TYPE_ITEM: {
                final Schedule schedule = (Schedule) get(position);

                if (schedule == null) {
                    notifyItemRemoved(position);

                    break;
                }


                final ViewHolders.ItemViewHolder itemHolder = (ViewHolders.ItemViewHolder) holder;


                final String subtitle1 = getString(R.string.title_hour_start);

                setText(itemHolder.subtitle1TextView, subtitle1);


                final String title1 = schedule.getStartTime();

                setText(itemHolder.title1TextView, title1);


                final String subtitle2 = getString(R.string.title_hour_finish);

                setText(itemHolder.subtitle2TextView, subtitle2);


                final String title2 = schedule.getEndTime();

                setText(itemHolder.title2TextView, title2);


                final String name = schedule.getName();

                itemHolder.setTextName(schedule.getName());


                setText(itemHolder.campusTextView, schedule.getCampus());


                @ColorInt
                final int colorPrimary = getColor(R.color.colorPrimary);


                for (final TextView dayTextView : itemHolder.getDayTextViews()) {
                    if (dayTextView.getBackground() != null) {
                        dayTextView.setBackground(null);
                    }

                    if (dayTextView.getCurrentTextColor() != colorPrimary) {
                        dayTextView.setTextColor(colorPrimary);
                    }
                }


                final Context context = getContext();


                final ArrayList<Schedule.Day> days = schedule.getDays();

                if (days != null && days.size() > 0) {
                    @ColorInt
                    final int textColor = getColor(android.R.color.white);

                    for (final Schedule.Day day : days) {
                        final TextView dayTextView = itemHolder.getDayTextView(day);

                        if (dayTextView == null) {
                            continue;
                        }

                        final Drawable background = ResourcesUtils
                                .getDrawable(context, R.drawable.weekday_oval);

                        dayTextView.setBackground(background);

                        dayTextView.setTextColor(textColor);

                        dayTextView.setOnLongClickListener(new View.OnLongClickListener() {

                            @Override
                            public boolean onLongClick(View view) {
                                showToast(day.getStringResId());

                                return true;
                            }

                        });
                    }
                }


                setText(itemHolder.classroomTextView, schedule.getClassroom());


                final OnClassClickListener clickListener = new OnClassClickListener
                        (mOnClassListener, itemHolder, schedule);

                itemHolder.container.setOnClickListener(clickListener);


                final View.OnLongClickListener longClickListener = new View.OnLongClickListener() {

                    @Override
                    public boolean onLongClick(View view) {
                        if (StringUtils.isAnyEmpty(title1, title2, name)) {
                            final String text = getString(R.string.title_not_available);

                            showToast(text);
                        } else {
                            final String text = name
                                    + StringUtils.LF + subtitle1 + StringUtils.SPACE + title1
                                    + StringUtils.LF + subtitle2 + StringUtils.SPACE + title2;

                            showToast(text);
                        }

                        return true;
                    }

                };

                itemHolder.container.setOnLongClickListener(longClickListener);


                if (itemHolder.circleButton == null) {
                    break;
                }


                itemHolder.setCircleButtonIcon(IconValue.ARROW_RIGHT);


                itemHolder.circleButton.setOnClickListener(clickListener);

                itemHolder.circleButton.setOnLongClickListener(longClickListener);


                new Handler().post(new Runnable() {

                    @Override
                    public void run() {
                        itemHolder.circleButton.requestLayout();
                    }

                });

                break;
            }

            default:
                break;
        }
    }


    public interface OnClassListener {

        void onClassClick(View view,
                          @NonNull
                          final ViewHolders.ItemViewHolder itemHolder,
                          @NonNull
                          final Schedule schedule);

    }


    public static class ViewHolders extends TimelineAdapter.ViewHolders {

        public static class ItemViewHolder extends BaseItemViewHolder {

            @BindView(R.id.campusTextView)
            TextView campusTextView;


            @BindView(R.id.sundayTextView)
            TextView sundayTextView;

            @BindView(R.id.mondayTextView)
            TextView mondayTextView;

            @BindView(R.id.tuesdayTextView)
            TextView tuesdayTextView;

            @BindView(R.id.wednesdayTextView)
            TextView wednesdayTextView;

            @BindView(R.id.thursdayTextView)
            TextView thursdayTextView;

            @BindView(R.id.fridayTextView)
            TextView fridayTextView;

            @BindView(R.id.saturdayTextView)
            TextView saturdayTextView;


            @BindView(R.id.classroomTextView)
            TextView classroomTextView;


            private ItemViewHolder(@NonNull View view) {
                super(view);

                ButterKnife.bind(this, view);

                setupComponents();
            }


            @Contract(pure = true)
            @Nullable
            private TextView getDayTextView(@NonNull final Schedule.Day day) {
                switch (day) {
                    case SUNDAY:
                        return sundayTextView;

                    case MONDAY:
                        return mondayTextView;

                    case TUESDAY:
                        return tuesdayTextView;

                    case WEDNESDAY:
                        return wednesdayTextView;

                    case THURSDAY:
                        return thursdayTextView;

                    case FRIDAY:
                        return fridayTextView;

                    case SATURDAY:
                        return saturdayTextView;

                    default:
                        return null;
                }
            }

            @NonNull
            private List<TextView> getDayTextViews() {
                final List<TextView> textViews = new ArrayList<>();

                textViews.add(sundayTextView);

                textViews.add(mondayTextView);

                textViews.add(tuesdayTextView);

                textViews.add(wednesdayTextView);

                textViews.add(thursdayTextView);

                textViews.add(fridayTextView);

                textViews.add(saturdayTextView);

                return textViews;
            }

        }

    }


    private static class OnClassClickListener implements View.OnClickListener {

        @NonNull
        private OnClassListener mClickListener;

        @NonNull
        private ViewHolders.ItemViewHolder mItemHolder;

        @NonNull
        private Schedule mSchedule;


        private OnClassClickListener(@NonNull
                                     final OnClassListener clickListener,
                                     @NonNull
                                     final ViewHolders.ItemViewHolder itemHolder,
                                     @NonNull
                                     final Schedule schedule) {
            mClickListener = clickListener;

            mItemHolder = itemHolder;

            mSchedule = schedule;
        }


        @Override
        public void onClick(View view) {
            mClickListener.onClassClick(view, mItemHolder, mSchedule);
        }

    }

}
