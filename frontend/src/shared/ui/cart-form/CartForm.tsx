import styles from './CartForm.module.css';
import { cn } from "@/shared/lib/utils";
import { CartFormProps } from './CartForm.props';


export default function CardFrom({...props} : CartFormProps ) {
return (
  <form  {...props} className={cn(styles['cart-form'])} >
    <input required name="text" type="text" aria-label="task text" placeholder="please type text" className={cn(styles['cart-input'])}/>
    <button className={cn(styles['cart-btn'])}>Add</button>
  </form>
)
}