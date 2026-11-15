import useTodos from '@/shared/lib/hooks/use-todos';
import CartForm from '../cart-form/CartForm';
import styles from './Cart.module.css';
import { cn } from '@/shared/lib/utils';
import { CART_ITEM_TYPES, CartItemTypes } from '../card-item/CartItem.model';
import CartItem from '../card-item/CartItem';
import CartSummary from '../card-summary/CardSummary';

export default function Card() {

  const {items, itemsState, addItem, removeItem, changeItem} = useTodos()

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    const data = new FormData(e.currentTarget);
    const text = data.get('text') as string;
    e.currentTarget.reset();
    addItem(text);
  }

  return (
  
    <div className={cn(styles['cart-wrapper'])}>
      <CartForm onSubmit = {handleSubmit}/>
      <CartSummary counts={itemsState}/>
      <ul className={cn(styles['cart-container'])}>
      {items.map((i) => {
        return <CartItem key={i.id} id={i.id} text={i.text} type={i.type} delete={removeItem} change={changeItem} />;
      })}
      </ul>
    </div>

  );
}


