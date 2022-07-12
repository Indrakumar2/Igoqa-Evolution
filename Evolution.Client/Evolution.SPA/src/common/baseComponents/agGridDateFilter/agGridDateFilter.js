import React, { Component, Fragment } from 'react';
import DatePicker from 'react-datepicker';
import moment from 'moment';
import { getlocalizeData } from '../../../utils/commonUtils';
const localConstant = getlocalizeData();

class AgGridDateFilter extends Component {
    constructor(props) {
        super(props);
        this.state = {
            date: null,
            
        };
    }
    onDateChanged = (data) => {        
        this.setState({ date: data });
        this.updateAndNotifyAgGrid(data);

    }
    getDate = () => {        
        if(this.state.date!=null){
            const a = new Date(this.state.date);
            return a;
        }
        
    }
    setDate(date) {    
        const a = moment(date).format("YYYY-MM-DD HH:mm:ss");
        this.setState({
            a
        });
    
    if(date === null ){
        this.setState({ date:null },
            
            );
        }
    }
    updateAndNotifyAgGrid(date) {      
        this.setState(
            {
            date,
        },
            this.props.onDateChanged
        );
    }
    clearDateFilters = (data) => {
        this.props.api.setFilterModel(null);
        this.props.api.onFilterChanged();
    }
    render() {
        return (
            <Fragment>
                <div className="datePickerHolder">
                    <DatePicker                                               
                        //showYearDropdown
                        //scrollableYearDropdown       //Defect Id 927 as per Mark E-Mail removed the Month & Year pick lists from all browsers 
                        //showMonthDropdown
                        //yearDropdownItemNumber={3}
                        autoComplete="off"
                        name={'agDatepicker'}
                        selected={this.state.date}
                        dateFormat={localConstant.commonConstants.UI_DATE_FORMAT}
                        className="browser-default"
                        onChange={this.onDateChanged}
                        disabled={null}
                        placeholderText={localConstant.commonConstants.FILTER_DATE_FORMAT}
                        disabledKeyboardNavigation
                    />                 
                </div>

            </Fragment>
        );
    }
}

export default AgGridDateFilter;