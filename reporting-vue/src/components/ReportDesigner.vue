<template>
    <div id="wrd1">...</div>
  </template>
  
  <script>
  export default {
    name: "WebReportDesigner",
    mounted() {
      this.$nextTick(function () {
        $("#wrd1")
          .telerik_WebReportDesigner({
            toolboxArea: {
              layout: "list", //Change to "grid" to display the contents of the Components area in a flow grid layout.
            },
            serviceUrl: "https://demos.telerik.com/reporting/api/reportdesigner/",
            report: "Barcodes Report.trdx",
          })
          .data("telerik_WebDesigner");
      });
    },
  };
  </script>
  
  <!-- Add "scoped" attribute to limit CSS to this component only -->
  <style scoped>
  #wrd1 {
    position: relative;
    width: 100%;
    height: 880px;
  }
  </style>