import React, { Component, Fragment } from 'react';
import dateUtil from '../../../utils/dateUtil';
import { getlocalizeData } from '../../../utils/commonUtils';
import moment from 'moment';
const localConstant = getlocalizeData();
class DateComponent extends Component {
    renderDate = () => {
        if (this.props.data[this.props.dataToRender]) {
            const momentdate = moment(this.props.data[this.props.dataToRender]);
            if (momentdate.isValid()) {
                return moment(this.props.data[this.props.dataToRender]).format(localConstant.commonConstants.UI_DATE_FORMAT);
            }
        }
        return '';
    }
    render() {
        return (
            <Fragment>   
       
                {this.props.colDef.cellRendererParams &&
                    this.props.colDef.cellRendererParams.type === "date-Time" ?
                    // moment.utc((this.props.data[this.props.dataToRender])).local().format(localConstant.commonConstants.AUDIT_DATE_FORMAT_TIME) :
                    moment((this.props.data[this.props.dataToRender])).format(localConstant.commonConstants.AUDIT_DATE_FORMAT_TIME) :
                    this.renderDate()
                    //As requested from client to show direct UTC datetime in audit, we made this changes
                }
            </Fragment>
        );
    }
}
export default DateComponent;