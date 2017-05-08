// Update-app 
// A vue instance to controll tracker's updating and editing functions

$(document).ready(function () {
    Vue.prototype.$http = axios;

    var app = new Vue({
        el: '#app',
        data: function () {
            var d = this.getIniData();
            return d;
        },
        beforeCreate: function() {

        },
        methods: {
            resetAlert: function () {
                this.alerts.warning = false;
                this.alerts.fail = false;
                this.alerts.success = false;
                this.alerts.submitting = false;
            },
            saveBtn: function (e) {
                e.preventDefault();
                this.resetAlert();
                this.alerts.submitting = true;
                // check whether data has been modified
                if (!_.isEqual(this.iniTrackerData, this.trackerData)) {
                    this.postData(this.trackerData);
                } else {
                    this.alerts.submitting = false;
                    this.alerts.warning = true;
                }
            },
            getIniData: function () {
                return {
                    title: PageGlobal.pageTitle,
                    message: 'This is built with vue',
                    updateUrl: PageGlobal.updateUrl,
                    getUrl: PageGlobal.getUrl,
                    selectedLeaid: PageGlobal.iniLeaid,
                    dataReady: false,
                    submitting: false,
                    alerts: {
                        success: false,
                        warning: false,
                        fail: false,
                        submitting: false
                    },
                    trackerData: {},
                    iniTrackerData: {}
                };
            },
            getData: function (url) {
                var that = this;
                this.$http.get(url)
                    .then(function (data) {
                        that.trackerData = Object.assign({}, data.data);
                        that.iniTrackerData = Object.assign({}, data.data);
                        //that.trackerData.submissionDate = that.frontEndDateFormat(that.trackerData.submissionDate);
                        //that.iniTrackerData.submissionDate = that.frontEndDateFormat(that.iniTrackerData.submissionDate);
                        that.dataReady = true;
                    })
                    .catch(function (e) {
                        console.log(e);
                    });
            },
            postData: function (data) {
                var that = this;
                var url = encodeURI(this.updateUrl);
                this.$http.post(url, data)
                    .then(function (response) {
                        //console.log(response);
                        if (response.status === 200) {
                            that.iniTrackerData = Object.assign({}, that.trackerData);
                            that.alerts.submitting = false;
                            that.alerts.success = true;
                        } else {
                            console.log(response.status);
                        }
                    })
                    .catch(function (e) {
                        that.alerts.warning = false;
                        that.alerts.fail = true;
                        that.alerts.success = false;
                    });
            },
            frontEndDateFormat: function (date) {
                return moment(date).format('MM/DD/YYYY');
            },
            serverDateFormat: function (date) {
                return moment(date, 'MM/DD/YYYY').format();
            }

        },
        created: function () {
            // get ini data from server
            var url = encodeURI(this.getUrl);
            this.getData(url);
        },
        updated: () => {
            $('.label-pop').popover({
                placement: 'top',
                html: true
            });
            $('.datepicker').datepicker();
        }
    });
});
