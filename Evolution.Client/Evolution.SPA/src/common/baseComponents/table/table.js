import React from 'react';
const Table = (props) => {
    const { columnData, headerData } = props;
    const rows = [];
    const headerColumns = [];
    columnData.map(data => {
        data.resourceSearchTechspecInfos.map(techData => {
            rows.push(<tr>
                <td>{techData.lastName}</td>
                <td>{techData.firstName}</td>
                <td>{techData.employmentType}</td>
                <td>{techData.scheduleStatus}</td>
                <td>{techData.distanceFromVenderInMile}</td>
                <td>{techData.distanceFromVenderInKm}</td>
                <td>{techData.isSupplier ? "Yes" : "No"}</td>
                <td>{techData.mobileNumber}</td>
                <td>{techData.email}</td>
                <td>{techData.googleAddress}</td>
                <td>{data.location}</td>
                <td>{(data.supplierInfo && data.supplierInfo.length > 0) ? data.supplierInfo[0].supplierName : ""}</td>
                {/* <td>{data.supplierInfo ? data.supplierInfo[0].supplierName : ""}</td> */}
            </tr>);
        });
    });
    headerData.map(header => {
        headerColumns.push(<th>{header}</th>);
    });

    return (
        <table id="exportTableId" border="1">
            <thead>
                <tr>
                    {headerColumns}
                </tr>
            </thead>
            <tbody>
                {rows}
            </tbody>
        </table>
    );
};

export default Table;