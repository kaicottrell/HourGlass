var barChartData = [];


function populateGraphs(data) {
    console.log(data);

    // grab the data passed in on page load
    var dataArray = Object.values(data);

    // create the array with the values for the graphs
    var barChartData = dataArray.map(function (templateDuration) {
        return {
            category: templateDuration.templateName,
            hours: templateDuration.totalDurationHours,
            color: templateDuration.templateColor
        };
    });


    // set the container height and wdith
    var barChartContainerWidth = 400; 
    var barChartContainerHeight = 350; 

    // main bar chart svg graph
    var barChartSvg = d3.select("#bar-chart-container")
        .append("svg")
        .attr("width", barChartContainerWidth)
        .attr("height", barChartContainerHeight)
        .append("g")
        .attr("transform", "translate(" + (100) + "," + (50) + ")");

    
    var xScale = d3.scaleBand()
        .domain(barChartData.map(function (d) { return d.category; }))
        .range([0, 325])
        .padding(0.1);

    var yScale = d3.scaleLinear()
        .domain([d3.max(barChartData, function (d) { return d.hours; }), 0])
        .range([0, 200]);

    // X axis with labels
    barChartSvg.append("g")
        .attr("transform", "translate(0,200)")
        .call(d3.axisBottom(xScale))
        .selectAll("text")
        .style("text-anchor", "end")
        .attr("dx", "-.8em")
        .attr("dy", ".15em")
        .attr("transform", "rotate(-30)")
        .style("font-size", "18px");

    // Y axis
    barChartSvg.append("g")
        .call(d3.axisLeft(yScale))


    // X axis label
    /*
    barChartSvg.append("text")
        .attr("transform", "translate(150,300)")
        .style("text-anchor", "middle")
        .style("font-size", "32px")
        .text("Activities");
    */

    // Y axis label
    barChartSvg.append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", -75)
        .attr("x", -95)
        .attr("dy", "1.1em")
        .style("text-anchor", "middle")
        .style("font-size", "32px")
        .text("Hours");

    // bars for the bar chart
    barChartSvg.selectAll("rect")
        .data(barChartData)
        .enter()
        .append("rect")
        .attr("x", function (d) { return xScale(d.category); })
        .attr("y", function (d) { return yScale(d.hours); }) 
        .attr("width", xScale.bandwidth())
        .attr("height", function (d) { return 200 - yScale(d.hours); }) 
        .attr("fill", function (d) { return d.color; });

    // Calculate the total sum of hours
    var totalHours = barChartData.reduce(function (sum, data) {
        return sum + data.hours;
    }, 0);

    // Convert hours to percentages
    barChartData.forEach(function (data) {
        data.percentage = (data.hours / totalHours) * 100;
    });

    // Set up the SVG container for the pie chart 
    var pieChartSvg = d3.select("#pie-chart-container")
        .append("svg")
        .attr("width", 400)
        .attr("height", 300)
        .style("margin-left", "75px")
        .append("g")
        .attr("transform", "translate(220,140)"); 

    // Pie chart data using the percentages 
    var pieChartData = barChartData.map(function (data) {
        return {
            value: data.percentage,
            color: data.color
        };
    });

    var pie = d3.pie().value(function (d) {
        return d.value;
    });
    var arc = d3.arc().innerRadius(0).outerRadius(105); 

    
    var paths = pieChartSvg.selectAll("path")
        .data(pie(pieChartData))
        .enter()
        .append("g")
        .attr("class", "arc");

    paths.append("path")
        .attr("d", arc)
        .attr("fill", function (d) {
            return d.data.color;
        });

    paths.append("text")
        .attr("transform", function (d) {
            var centroid = arc.centroid(d);
            return "translate(" + centroid[0] + "," + centroid[1] + ")";
        })
        .attr("dy", "0.75em")
        .attr("text-anchor", "middle")
        .style("fill", function (d) {
            // Check if the segment color is black
            return d.data.color === "#000001" ? "white" : "black";
        })
        .text(function (d) {
            return d.data.value.toFixed(0) + "%";
        });

   
    
    var legend = d3.select("#pie-chart-container")
        .append("svg")
        .attr("width", 450) 
        .attr("height", 100) 
        .style("margin-left", "55px")
        .attr("transform", "translate(50,0)");

    legend.selectAll("rect")
        .data(barChartData)
        .enter()
        .append("rect")
        .attr("x", function (d, i) {
            return (i % 2) * 225;
        })
        .attr("y", function (d, i) {
            return Math.floor(i / 2) * 25;
        })
        .attr("width", 20)
        .attr("height", 20)
        .attr("fill", function (d) { return d.color; });

    legend.selectAll("text")
        .data(barChartData)
        .enter()
        .append("text")
        .attr("x", function (d, i) {
            return (i % 2) * 225 + 30; 
        })
        .attr("y", function (d, i) {
            return Math.floor(i / 2) * 25 + 14; 
        })
        .text(function (d) { return d.category; });

    
    // Display percentages and rank in the div
    var weeklyStatsDiv = d3.select("#weekly-stats");

    barChartData.sort(function (a, b) {
        return b.hours - a.hours;
    });

    var table = weeklyStatsDiv.append("table").attr("class", "stats-table")
        .style("margin-left", "75px")
        .style("margin-top", "50px");

    // Append the header row
    var thead = table.append("thead");
    thead.append("tr")
        .selectAll("th")
        .data(["Rank", "Category", "Hours"])
        .enter()
        .append("th")
        .style("font-size", "28px")
        .style("font-weight", "bold")
        .style("text-align", "center")
        .style("border", "1px solid #dddddd")  
        .text(function (column) { return column; });

    // Append the rows
    var tbody = table.append("tbody");
    barChartData.forEach(function (data, index) {
        var row = tbody.append("tr")
            .style("border-bottom", "1px solid #dddddd"); 
        row.append("td")
            .text(index + 1)
            .style("text-align", "center")
            .style("font-size", "24px")
            .style("border", "1px solid #dddddd"); 
        row.append("td")
            .text(data.category)
            .style("text-align", "center")
            .style("font-size", "24px")
            .style("border", "1px solid #dddddd"); 
        row.append("td")
            .text(data.hours)
            .style("text-align", "center")
            .style("font-size", "24px")
            .style("border", "1px solid #dddddd");
    });
}
