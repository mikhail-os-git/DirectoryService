import styles from './CartSummary.module.css';
import { cn } from '@/shared/lib/utils';
import { CART_ITEM_TYPES, CartItemTypes } from "../card-item/CartItem.model";
import { CartSummaryProps } from './CartSummary.props';


export default function CartSummary(props: CartSummaryProps ) {
  return (
    <div className={styles.summary}>

      <div className={styles['chips']}>
        {CART_ITEM_TYPES.map((t) => (
          <span key={t} className={cn(styles['chip'], styles[t])}>
            {t} <b>{props.counts[t]}</b>
          </span>
        ))}
      </div>
    </div>
  );
}