import React from 'react';
import PropTypes from 'prop-types';
import Panel from '../../common/baseComponents/panel';
import { ProcessSchema } from '../../common/generateFields/generateFields';

class SearchFilter extends React.Component {
    constructor(props){
        super(props);
        this.state = {
            isPanelOpen:true,
        };
        this.schema =  props.fields.subComponents;
    }

    panelClick = (e) => {
        this.setState((state)=>{
            return {
                isPanelOpen: !state.isPanelOpen
            };
        });
    }

    render() {
        const { 
            subtitle, 
            heading, 
            colSize, 
            searchFields, 
            filterFieldsData,
            resetHandler,
            submitHandler,
            setData,
            isArrow,
            csvRef } = this.props;
        return (
            <Panel colSize={colSize}
                isArrow={isArrow}
                heading={heading}
                subtitle={subtitle}
                onpanelClick={(e) => this.panelClick(e)}
                isopen={this.state.isPanelOpen} >
                <form id="contractSearch"
                    onSubmit={submitHandler}
                    // onReset={resetHandler} 
                    autoComplete="off">
                    <ProcessSchema
                        childFields={searchFields.subComponents}
                        data={filterFieldsData}
                        setValues={setData}
                        csvRef={csvRef}
                    />
                </form>
            </Panel>
        );
    }
}

export default SearchFilter;

SearchFilter.propTypes = {
    fields: PropTypes.array
};

SearchFilter.defaultProps = {
    fields: []
};
