﻿function view()
{
    return function run(renderTo, pagedata)
    {
        var eleId = "chart" + Math.random() + new Date().getTime();
        $(renderTo).attr("id", eleId);
        pbc.web.loader('chart', show);

        function show()
        {
            // 基于准备好的dom，初始化echarts实例
            var myChart = echarts.init($(renderTo).get(0));

            // 指定图表的配置项和数据
            var xAxisData = [];
            var data1 = [];
            var data2 = [];
            for (var i = 0; i < 100; i++)
            {
                xAxisData.push('类目' + i);
                data1.push((Math.sin(i / 5) * (i / 5 - 10) + i / 6) * 5);
                data2.push((Math.cos(i / 5) * (i / 5 - 10) + i / 6) * 5);
            }

            var option = {
                title: {
                    text: '柱状图动画延迟'
                },
                legend: {
                    data: ['bar', 'bar2'],
                    align: 'left'
                },
                toolbox: {
                    // y: 'bottom',
                    feature: {
                        magicType: {
                            type: ['stack', 'tiled']
                        },
                        dataView: {},
                        saveAsImage: {
                            pixelRatio: 2
                        }
                    }
                },
                tooltip: {},
                xAxis: {
                    data: xAxisData,
                    silent: false,
                    splitLine: {
                        show: false
                    }
                },
                yAxis: {
                },
                series: [{
                    name: 'bar',
                    type: 'bar',
                    data: data1,
                    animationDelay: function (idx)
                    {
                        return idx * 10;
                    }
                }, {
                    name: 'bar2',
                    type: 'bar',
                    data: data2,
                    animationDelay: function (idx)
                    {
                        return idx * 10 + 100;
                    }
                }],
                animationEasing: 'elasticOut',
                animationDelayUpdate: function (idx)
                {
                    return idx * 5;
                }
            };


            // 使用刚指定的配置项和数据显示图表。
            myChart.setOption(option);

        }

    };
}