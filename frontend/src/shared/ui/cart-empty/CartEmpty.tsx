import styles from './CartEmpty.module.css';
import { CartIcon } from '../cart-icon/CartIcon';

const emptyIcon =
  'M15 4h3a1 1 0 0 1 1 1v15a1 1 0 0 1-1 1H6a1 1 0 0 1-1-1V5a1 1 0 0 1 1-1h3m0 3h6m-6 5h6m-6 4h6M10 3v4h4V3h-4Z';

export default function CartEmpty() {
  return (
    <div className={styles['empty']}>
      <CartIcon paths={[emptyIcon]} className={styles['icon']} />
      <p className={styles['title']}>Not tasks exist</p>
      <p className={styles['hint']}>Add your first task using the form above</p>
    </div>
  );
}