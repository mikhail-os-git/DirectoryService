import styles from './CartItem.module.css';
import { cn } from "@/shared/lib/utils";
import { CartIconClose, CartIcon} from '../cart-icon/CartIcon';
import { CART_ITEM_TYPES, CartItemTypes, ICartItem, ICONS } from './CartItem.model';

type CardItemProps = ICartItem & {delete: (id: number) => void, change: (id: number, type: CartItemTypes) => void};

export default function CartItem({type = 'default', ...props}: CardItemProps ) {
  return (
        <li className={cn(styles['cart-item'], styles[type])}>
          <div className={cn(styles['cart-item-content'])}>
            <CartIcon paths={ICONS[type]}/>
            <div className={cn(styles['cart-item-text'])}>{props.text}</div>
          </div>
            <div className={styles['cart-item-actions']}>
          <select
            value={type}
            onChange={(e) => props.change(props.id ,e.target.value as CartItemTypes)}
            className={styles['cart-item-select']}
          >
            {CART_ITEM_TYPES.map((t) => (
              <option key={t} value={t}>{t}</option>
            ))}
          </select>
          <CartIconClose onDelete={() => props.delete(props.id)} />
        </div>
          <div className={cn(styles['cart-item-progress-bar'])} />
        </li>
  )
}

