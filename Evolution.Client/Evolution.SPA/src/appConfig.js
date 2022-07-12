export const configuration = {
    selectedCompany: sessionStorage.getItem("CompanyCode"), // to get tab specific company code : user can select different home company in each tab
    apiBaseUrl: process.env.REACT_APP_API_BASE_URL,
    extranetURL: process.env.REACT_APP_EXTRANET_URL,
    fileLimit: process.env.REACT_APP_UPLOAD_FILE_LIMIT, //|| 204800, // File Limit is 200 MB && 204800(is in KB),
    idleTimeOut: process.env.REACT_APP_IDLE_TIMEOUT_IN_SECONDS, //|| 1800, // Application Idle Time Out in Seconds
    idleWaitingTime: process.env.REACT_APP_IDLE_WAITING_TIME_IN_MILLISECONDS, //|| 60000, // Application idle time expired then auto Logout in milliseconds 
    allowedFileFormats: '.txt,.pdf,.doc,.docx,.xls,.xlsx,.msg,.png,.jpg,.jpeg,.gif,.csv,.zip,.rar,.mp4,.mp3,.avi,.mvi,.wmv,.mpeg,.html,.htm,.ppt,.pptx',
    version: process.env.REACT_APP_APPLICATION_VERSION_NUMBER
};