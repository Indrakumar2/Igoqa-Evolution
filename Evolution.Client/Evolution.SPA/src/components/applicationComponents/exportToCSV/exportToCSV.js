// import React, { Component, Fragment } from 'react';
// import { getlocalizeData } from '../../../utils/commonUtils';
// import { CSVLink, CSVDownload } from "react-csv";

// const localConstant = getlocalizeData();

// class ExportToCSV extends Component {
//     constructor(props) {
//         super(props);
//     }
//     render() {
//         const { csvExportClick, data, header, csvRef, buttonClass, csvClass, filename } = this.props;
//         return (
//             <Fragment>
//                 <button onClick={csvExportClick} className={buttonClass}>Export</button>
//                 {
//                     data && header ?
//                         <CSVLink data={data != null ? data : []} headers={header != null ? header : []} filename={filename} className={csvClass}
//                             // onClick={props.csvExportClick} //asyncOnClick={true}
//                             ref={csvRef}
//                         >
//                             {localConstant.commonConstants.BTN_EXPORT}
//                         </CSVLink>
//                         : null
//                 }
//             </Fragment>
//         );
//     }
// }

// export default ExportToCSV;
