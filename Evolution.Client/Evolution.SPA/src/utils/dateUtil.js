import moment from 'moment';

const dateUtil = {
    formatDate(inputFormat, separator) {
        function pad(s) { return (s < 10) ? '0' + s : s; }
        const d = new Date(inputFormat);
        if (inputFormat) {
            return [ pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear() ].join(separator);
        }
        else {
            return null;
        }
    },

    postDateFormat(inputFormat, separator) {
        function pad(s) { return (s < 10) ? '0' + s : s; }
        const d = new Date(inputFormat);
        if (inputFormat) {
            return [ d.getFullYear(), pad(d.getMonth() + 1), pad(d.getDate()) ].join(separator);
        }
        else {
            return null;
        }
    },
    getLocalTime(seconds) {
        const dateWithUTC = new Date(seconds * 1000);
        return Date.UTC(
            dateWithUTC.getFullYear(),
            dateWithUTC.getMonth(),
            dateWithUTC.getDate(),
            dateWithUTC.getHours(),
            dateWithUTC.getMinutes(),
            dateWithUTC.getSeconds());
    },
    isValidDate(date) {
        const datePattern = /(^(((0[1-9]|1[0-9]|2[0-8])[-](0[1-9]|1[012]))|((29|30|31)[-](0[13578]|1[02]))|((29|30)[-](0[4,6,9]|11)))[-](19|[2-9][0-9])\d\d$)|(^29[-]02[-](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)/;
        if (datePattern.test(date)) {
            return true;
        } else {
            return false;
        }
    },
    isUIValidDate(date) {
        const datePattern =  /^(([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])(-)(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(-)((19|20)\d{2}))$/i;
        if (datePattern.test(date)) {
            return true;
        } else {
            return false;
        }
    },
    defaultMoment(date) {
        return date ? moment(date) : '';
    },
    dateInBetween(date, startDate, endDate) {
        return moment(date).isBetween(startDate, endDate, 'day', []);
    },
    comparator(filterDate, cellValue) {
        const dateAsString = moment(cellValue).format('DD-MM-YYYY');
        const dateParts = dateAsString.split("-");
        const cellDate = new Date(Number(dateParts[2]), Number(dateParts[1]) - 1, Number(dateParts[0]));
        if (filterDate.getTime() === cellDate.getTime()) {
            return 0;
        }
        if (cellDate < filterDate) {
            return -1;
        }
        if (cellDate > filterDate) {
            return 1;
        }
    },
    getEpochSeconds(){
       return Math.floor(new Date().getTime()/1000.0);
    },
    getTimeZoneOffsetAsString() {
        const timezone_offset_min = new Date().getTimezoneOffset();
        let offset_hrs = parseInt(Math.abs(timezone_offset_min / 60));
        let offset_min = Math.abs(timezone_offset_min % 60);
        let timezone_standard;

        if (offset_hrs < 10)
            offset_hrs = '0' + offset_hrs;

        if (offset_min < 10)
            offset_min = '0' + offset_min;

        // Add an opposite sign to the offset
        // If offset is 0, it means timezone is UTC
        if (timezone_offset_min < 0)
            timezone_standard = '+' + offset_hrs + ':' + offset_min;
        else if (timezone_offset_min > 0)
            timezone_standard = '-' + offset_hrs + ':' + offset_min;
        else if (timezone_offset_min == 0)
            timezone_standard = 'Z';
        return timezone_standard;
    },

    // getWeekStartDate(date){
    //     const dateIndex = date.getDate() - date.getDay();
    //     return new Date(date.setDate(dateIndex)).toUTCString();
    // },

    getWeekStartDate(date) {
        const weekMap = [ 6, 0, 1, 2, 3, 4, 5 ];
        const now = new Date(date);
        now.setHours(0, 0, 0, 0);
        const monday = new Date(now);
        monday.setDate(monday.getDate() - weekMap[monday.getDay()]);
        return monday;
    },
    getWeekEndDate(date) {
        const weekMap = [ 6, 0, 1, 2, 3, 4, 5 ];
        const now = new Date(date);
        now.setHours(0, 0, 0, 0);
        const sunday = new Date(now);
        sunday.setDate(sunday.getDate() - weekMap[sunday.getDay()] + 6);
        sunday.setHours(23, 59, 59, 999);
        return sunday;
    },

    // getWeekEndDate(date){
    //     const dateIndex = date.getDate() - date.getDay();
    //     const lastIndex = dateIndex + 6;
    //     return new Date(date.setDate(lastIndex)).toUTCString();
    // }
};
export default dateUtil;
