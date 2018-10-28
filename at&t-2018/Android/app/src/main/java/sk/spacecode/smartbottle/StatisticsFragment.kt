package sk.spacecode.smartbottle

import android.os.Bundle
import android.support.v4.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import kotlinx.android.synthetic.main.fragment_statistics.*
import org.eazegraph.lib.models.BarModel
import org.eazegraph.lib.charts.BarChart
import org.eazegraph.lib.models.ValueLinePoint
import org.eazegraph.lib.models.ValueLineSeries
import org.eazegraph.lib.charts.ValueLineChart






class StatisticsFragment : Fragment() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {

        val view = inflater.inflate(R.layout.fragment_statistics, container, false)

        val mCubicValueLineChart = view!!.findViewById(R.id.statistic_chart_week) as ValueLineChart
        val series = ValueLineSeries()
        series.color = -0xa9480f
        for (i in 20..27) {
            series.addPoint(ValueLinePoint(i.toString() + ".10", (0.5 + Math.random() * (1.5 - 0.5)).toFloat()))
        }
        mCubicValueLineChart.addSeries(series)
        mCubicValueLineChart.startAnimation()


        val mCubicValueLineChart2 = view!!.findViewById(R.id.statistic_chart_month) as ValueLineChart
        val series2 = ValueLineSeries()
        series2.color = -0xa9480f
        for (i in 1..27) {
            series2.addPoint(ValueLinePoint(i.toString() + ".10", (0.5 + Math.random() * (1.5 - 0.5)).toFloat()))
        }
        mCubicValueLineChart2.addSeries(series2)
        mCubicValueLineChart2.startAnimation()

        return view
    }

    companion object {
        @JvmStatic
        fun newInstance() =
            StatisticsFragment()
    }
}
