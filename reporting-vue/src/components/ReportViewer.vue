<template>
    <div class="hello">
      <h1></h1>
      <div id="reportViewer1">loading...</div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'ReportViewer',
    data () {
      return {
        msg: 'Welcome to Your Vue.js App'
      }
    },
    mounted () {
      this.$nextTick(function () {
        $('#reportViewer1')
          .telerik_ReportViewer({
            serviceUrl: 'http://demos.telerik.com/reporting/api/reports/',
            reportSource: {
              report: 'Dashboard.trdx'
            },
            viewMode: telerikReportViewer.ViewModes.INTERACTIVE,
            scaleMode: telerikReportViewer.ScaleModes.SPECIFIC,
            scale: 1.0,
            sendEmail: { enabled: true }
          })
      })
    }
  }
  </script>
  
  <!-- Add "scoped" attribute to limit CSS to this component only -->
  <style scoped>
    body {
        font-family: Verdana, Arial;
        margin: 5px;
    }
  
    #reportViewer1 {
        position: relative;
        width: 80vw;
        height: 1000px;
    }
  </style>