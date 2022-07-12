import React,{ Component,Fragment } from 'react';
import { connect } from "react-redux";
import { isEmpty } from '../../../../utils/commonUtils';
import { StarRating } from 'react-star-rating-input';

class CustomStarRating extends Component{
    render(){  

        // const { equipmentKnowledge } = this.props.data;
       
        return(           
            <Fragment>
                {this.props.data && this.props.data.rating ? <StarRating  value={this.props.data.rating} /> : null}

            {/* {!isEmpty(equipmentKnowledge) ?                
                    <div className='row mb-0'>                      
                         {equipmentKnowledge && equipmentKnowledge.map((data,i) =>
                           <Fragment key={i}> <div className="col customDiv" key={i}>
                                               <div className="col s12">{data.value} </div>
                                                <div className="col s12">
                                                    <StarRating key={i}  value={data.rating} />
                                                </div>
                                              </div> 
                            </Fragment>
                        )}
                    </div>                                
               :null
            } */}
                     
        </Fragment>
        );
    }
}

export default connect(null, {})(CustomStarRating);