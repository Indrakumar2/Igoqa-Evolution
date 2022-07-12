import React, { Component } from 'react';
import { getlocalizeData } from '../../../utils/commonUtils';
import { required } from '../../../utils/validator';
const localConstant = getlocalizeData();

class ExportLink extends Component {

    exportClick = () => {
        if (this.props.data && this.props.data.epin) {
                this.props.editAction(this.props.data.epin, this.props.colDef.isChevronExport);
        }
    }

    render() {
        const { colDef,data } = this.props;
        return (
            !required(data.epin) && <a className={"waves-effect waves-light "} href="javascript:void(0)" title={colDef.isChevronExport ? "Chevron Export" : "Export"}>
                <i className="zmdi zmdi-upload zmdi-hc-lg" onClick={this.exportClick}></i>
            </a>
        );
    }
}
export default ExportLink;
