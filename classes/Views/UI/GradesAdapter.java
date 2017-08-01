import org.apache.commons.lang3.StringUtils;

import java.text.DecimalFormat;
import java.util.List;

import butterknife.BindView;
import butterknife.ButterKnife;

public class GradesAdapter extends TimelineAdapter {

    private static final String FORMAT_GRADE = "###.##";


    private final float MAX_PARTIAL_1;

    private final float MAX_PARTIAL_2;

    private final float MAX_PARTIAL_3;

    private final float MAX_TOTAL;


    public GradesAdapter(@NonNull
                         final Context context,
                         @NonNull
                         final List<Grade> list) {
        super(context, list);

        final Grade grade = list.get(0);

        if (grade != null) {
            MAX_PARTIAL_1 = grade.getPartial1();

            MAX_PARTIAL_2 = grade.getPartial2();

            MAX_PARTIAL_3 = grade.getPartial3();

            MAX_TOTAL = grade.getTotal();

            list.remove(grade);
        } else {
            MAX_PARTIAL_1 = MAX_PARTIAL_2 = MAX_PARTIAL_3 = MAX_TOTAL = 0;
        }
    }


    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        switch (viewType) {
            case VIEW_TYPE_ITEM: {
                final Context context = getContext();

                final View view = LayoutInflater.from(context)
                        .inflate(R.layout.item_grade, parent, false);

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

                final String title = getString(R.string.title_total);

                setText(headerHolder.titleTextView, title);


                final String description = getString(R.string.description_student_grades);

                setText(headerHolder.descriptionTextView, description);

                break;
            }

            case VIEW_TYPE_ITEM: {
                final Grade grade = (Grade) get(position);

                if (grade == null) {
                    notifyItemRemoved(position);

                    break;
                }


                final ViewHolders.ItemViewHolder itemHolder = (ViewHolders.ItemViewHolder) holder;


                final String subtitle1 = "Cursando";

                setText(itemHolder.subtitle1TextView, subtitle1);


                final String title1 = getFormatGrade(grade.getTotal());

                setText(itemHolder.title1TextView, title1);


                final String subtitle2 = getString(R.string.subtitle_grade_out_for);

                setText(itemHolder.subtitle2TextView, subtitle2);


                final String title2 = getFormatGrade(MAX_TOTAL);

                setText(itemHolder.title2TextView, title2);


                final String name = grade.getName();

                itemHolder.setTextName(name);


                setTextGrade(itemHolder.partial1TextView, grade.getPartial1(), MAX_PARTIAL_1);

                setTextGrade(itemHolder.partial2TextView, grade.getPartial2(), MAX_PARTIAL_2);

                setTextGrade(itemHolder.partial3TextView, grade.getPartial3(), MAX_PARTIAL_3);


                itemHolder.container.setOnLongClickListener(new View.OnLongClickListener() {

                    @Override
                    public boolean onLongClick(View view) {
                        if (StringUtils.isAnyEmpty(subtitle1, title1, subtitle2, title2, name)) {
                            final String text = getString(R.string.title_not_available);

                            showToast(text);
                        } else {
                            final String text = name
                                    + StringUtils.LF + subtitle1
                                    + StringUtils.LF + title1
                                    + StringUtils.SPACE + subtitle2
                                    + StringUtils.SPACE + title2;

                            showToast(text);
                        }

                        return true;
                    }

                });

                break;
            }

            default:
                break;
        }
    }


    @NonNull
    private String getFormatGrade(final float grade) {
        return new DecimalFormat(FORMAT_GRADE).format(grade);
    }

    @NonNull
    private String getFormatTotalGrade(final float grade, final float max) {
        final String format = getString(R.string.format_grade);

        return String.format(format, getFormatGrade(grade), getFormatGrade(max));
    }


    private void setTextGrade(@NonNull
                              final TextView textView, final float grade, final float max) {
        final String total = getFormatTotalGrade(grade, max);

        setText(textView, total);
    }


    static class ViewHolders extends TimelineAdapter.ViewHolders {

        static class ItemViewHolder extends BaseItemViewHolder {

            @BindView(R.id.partial1TextView)
            TextView partial1TextView;

            @BindView(R.id.partial2TextView)
            TextView partial2TextView;

            @BindView(R.id.partial3TextView)
            TextView partial3TextView;


            private ItemViewHolder(@NonNull View view) {
                super(view);

                ButterKnife.bind(this, view);

                setupComponents();
            }

        }

    }

}
