import { useEffect, useRef, useState, forwardRef, useImperativeHandle } from 'react'

// Importing altcha package will introduce a new element <altcha-widget>
import 'altcha'

const Altcha = forwardRef(({ onStateChange }, ref) => {
  const widgetRef = useRef(null)
  const [value, setValue] = useState(null)

  useImperativeHandle(ref, () => {
    return {
      get value() {
        return value
      }
    }
  }, [value])

  useEffect(() => {
    const handleStateChange = (ev) => {
      if ('detail' in ev) {
        setValue(ev.detail.payload || null)
        onStateChange?.(ev)
      }
    }

    const { current } = widgetRef

    if (current) {
      current.addEventListener('statechange', handleStateChange)
      return () => current.removeEventListener('statechange', handleStateChange)
    }
  }, [onStateChange])

  /* Configure your `challengeurl` and remove the `test` attribute, see docs: https://altcha.org/docs/website-integration/#using-altcha-widget  */
  return (
    <altcha-widget
          ref={widgetRef}
          style={{
              '--altcha-max-width': '100%',
          }}
          challengeurl="https://localhost:7135/challengeSelfHosted"
          debug
          auto="off"
          expire={30000}
          refetchonexpire={false}
          hidelogo={true}
          hidefooter={false}
          strings={JSON.stringify({
              "footer": "Test",
          })}
    ></altcha-widget>
  )
})

export default Altcha
