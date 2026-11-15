import styles from './CartIcon.module.css';
import { cn } from "@/shared/lib/utils";

type CartIconProps = React.ComponentProps<'div'> & {
  paths: string[];
};

export function CartIcon({ paths, className, ...props }: CartIconProps) {
  return (
    <div {...props} className={cn(styles['cart-item-icon'], className)}>
      <svg aria-hidden="true" fill="none" viewBox="0 0 24 24" width={20} height={20}>
          {paths.map((d) => (
            <path
              key={d}
              stroke="currentColor"
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d={d} />
          ))}
      </svg>
    </div>
  );
}
export function CartIconClose({ onDelete }: { onDelete: () => void }) {
  const closeIcon = 'M6 18 17.94 6M18 18 6.06 6';
  return (
    <CartIcon
      paths={[closeIcon]}
      className={styles['cart-item-close']}
      onClick={onDelete}
    />
  );
}
