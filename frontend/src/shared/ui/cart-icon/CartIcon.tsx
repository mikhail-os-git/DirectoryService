import styles from './CartIcon.module.css';
import { cn } from "@/shared/lib/utils";
import { SvgPath } from './SvgPath';

export type CartIconProps = React.ComponentProps<'div'> & {
  paths: string[];
};

export function CartIcon({ paths, ...props }: CartIconProps) {
  return (
    <div {...props} className={cn(styles['icon'])}>
      <SvgPath paths={paths}/>
    </div>
  );
}
interface CardIconCloseProps extends React.ComponentProps<'button'> {
  onDelete: () => void;
}

export function CartIconClose(props: CardIconCloseProps) {
  const closeIcon = 'M6 18 17.94 6M18 18 6.06 6';
  return (
    <button className={cn(styles['icon'], styles['close'])} onClick={props.onDelete} >
      <SvgPath paths={[closeIcon]}/>
    </button>
  );
}
